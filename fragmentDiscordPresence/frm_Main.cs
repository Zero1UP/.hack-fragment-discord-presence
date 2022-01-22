using Binarysharp.MemoryManagement;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace fragmentDiscordPresence
{
    public partial class frm_Main : Form
    {
        private const string PCSX2PROCESSNAME = "pcsx2";
        MemorySharp mem = null;
        bool sessionStarted = false;
        public DiscordRpcClient client;
        private  RichPresence defaultPresence = new RichPresence()
        {
            Details = "Not currently in a server.",
            State = "Not currently in an area.",
            Assets = new Assets()
            {
                LargeImageKey = "default",
                LargeImageText = "Not online",
                SmallImageKey = "default"
            },
            Timestamps = null,
            Party = null
        };
        private RichPresence presence = new RichPresence();
        public frm_Main()
        {
            InitializeComponent();
            client = new DiscordRpcClient("657845824927170580");
            client.Initialize();
            client.SetPresence(defaultPresence);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        }

        private void tmr_GameData_Tick(object sender, EventArgs e)
        {
            try
            {
                Process pcsx2 = Process.GetProcesses().First(p => p.ProcessName.StartsWith(PCSX2PROCESSNAME));
                long pcsx2Offset = GameHelper.GetPcsx2Offset(pcsx2);
                IntPtr pcsx2Intptr = new IntPtr(pcsx2Offset);
                
                mem = new MemorySharp(pcsx2);
                string areaWord1 = "";
                string areaWord2 = "";
                string areaWord3 = "";

                if (mem.IsRunning)
                {
                    if (mem.Read<byte>(IntPtr.Add(pcsx2Intptr,GameHelper.CONNECTED_TO_AS_ADDRESS), 4, false).First() == 0x01)
                    {

                        //Get party count
                        //Not really useful right now since I can't seem to get this to display
                        int partyCount = mem.Read<byte>(IntPtr.Add(pcsx2Intptr,GameHelper.PARTY_COUNT_ADDRESS), false);

                        //Get the server name
                        string serverName = ByteConverstionHelper.converyBytesToSJIS(mem.Read<byte>(IntPtr.Add(pcsx2Intptr,GameHelper.SERVER_NAME_ADDRESS), 20, false));

                        //Get the town you are in.
                        //This address actually shows the current zone ccs file. So when you zone into an area this will update to that zone's CCS file
                        //Could be useful to display the area you are in but using the default assets wouldn't be telling of where the player actually is.
                        string townFileName = mem.ReadString(IntPtr.Add(pcsx2Intptr,GameHelper.ZONE_CCS_FILE_LOADED),Encoding.Default,false,12).Split('.')[0].ToLower();

                        //Don't want the timer to continue if the player is just sitting in town.
                        if (townFileName.ToLower().Contains("town"))
                        {
                            presence.Timestamps = new Timestamps()
                            {
                                Start = null

                            };                       
                            sessionStarted = false;
                        }

                        //Check to see if we are in a zone. (Could technically use townFileName, but I haven't done extensive testing with that to see if it is
                        // always a zone file name that is loaded there.)

                        if (mem.Read<byte>(IntPtr.Add(pcsx2Intptr,GameHelper.IN_AREA_ADDRESS), 4, false).First() == 0x01)
                        {
                            //Get the pointer addresses and add their offsets to them
                            IntPtr areawordPointerAddress =
                                new IntPtr(mem.Read<int>(IntPtr.Add(pcsx2Intptr, GameHelper.AREA_WORD_POINTER), false) +
                                           pcsx2Offset);
                            IntPtr areaword1Pointer =
                                new IntPtr(mem.Read<int>(
                                    areawordPointerAddress + GameHelper.AREA_WORD_ONE_POINTER_OFFSET, false) + pcsx2Offset);
                            IntPtr areaWord2Pointer =
                                new IntPtr(mem.Read<int>(
                                    areawordPointerAddress + GameHelper.AREA_WORD_TWO_POINTER_OFFSET, false) + pcsx2Offset);
                            IntPtr areaWord3Pointer =
                                new IntPtr(mem.Read<int>(
                                    areawordPointerAddress + GameHelper.AREA_WORD_THREE_POINTER_OFFSET, false) + pcsx2Offset);

                            //Get the keywords.
                            areaWord1 = ByteConverstionHelper.converyBytesToSJIS(mem.Read<byte>(areaword1Pointer, 12, false)).Substring(4);
                            areaWord2 = ByteConverstionHelper.converyBytesToSJIS(mem.Read<byte>(areaWord2Pointer, 12, false)).Substring(4);
                            areaWord3 = ByteConverstionHelper.converyBytesToSJIS(mem.Read<byte>(areaWord3Pointer, 12, false)).Substring(4);
                            townFileName = "default";
                            //Check to see if the game is started (if the player is in a zone) so the timer won't constantly reset
                            if (!sessionStarted)
                            {
                                presence.Timestamps = new Timestamps()
                                {
                                    Start = DateTime.UtcNow

                                };

                                sessionStarted = true;
                            }
                        }
                        setPresence(serverName, areaWord1, areaWord2, areaWord3, partyCount, townFileName);
                    }
                }
                else
                {
                    client.SetPresence(defaultPresence);
                    sessionStarted = false;
                }


            }
            catch (Exception)
            {

                client.SetPresence(defaultPresence);
                sessionStarted = false;
            }
        }
        private void setPresence(string serverName, string word1, string word2, string word3, int playerCount,string zone)
        {
            presence.Details ="Area Server: " + serverName;
            presence.State = "Not currently in an area.";
            //Only checking the first word. If this isn't set then it's save to assume that we weren't in a zone
            //Used to display the area words however since adding the party count's they end up not showing because the world list was too long.
            if (word1 !="")
            {
                presence.State = "Currently in field.";
                // "Area: " + word1 + " " + word2 + " " + word3;
            }

            presence.Assets = new Assets();    
            presence.Assets.LargeImageKey = zone;
            presence.Assets.SmallImageKey = zone;

            presence.Party = new Party()
            {
                ID = new Guid().ToString(),
                Size = playerCount,
                Max = 3,
                Privacy = Party.PrivacySetting.Private

            };
            presence.HasParty();
            client.SetPresence(presence);
        }

        private void frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Dispose();
        }
    }
}
