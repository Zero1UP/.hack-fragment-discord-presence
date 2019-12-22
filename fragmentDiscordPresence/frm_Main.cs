using DiscordRPC;
using Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fragmentDiscordPresence
{
    public partial class frm_Main : Form
    {
        private const string PCSX2PROCESSNAME = "pcsx2";
        bool pcsx2Running = false;
        Mem m = new Mem();
        bool gameStarted = false;
        public DiscordRpcClient client;
        private static RichPresence presence = new RichPresence()
        {
            Details = "Not currently in an area.",
            State = "Not currently in a server.",
            Assets = new Assets()
            {
                LargeImageKey = "default",
                LargeImageText = "Not online",
                SmallImageKey = "default"
            }
        };

        public frm_Main()
        {
            InitializeComponent();
            client = new DiscordRpcClient("657845824927170580");
            client.Initialize();
            client.SetPresence(presence);

        }

        private void tmr_PCSX2Check_Tick(object sender, EventArgs e)
        {
            Process[] pcsx2 = Process.GetProcessesByName(PCSX2PROCESSNAME);

            if (pcsx2.Length > 0)
            {
                lbl_PCSX2.Text = "PCSX2 Detected";
                lbl_PCSX2.ForeColor = Color.FromArgb(20, 192, 90);
                pcsx2Running = true;
            }
            else
            {
                lbl_PCSX2.Text = "Waiting for PCSX2...";
                lbl_PCSX2.ForeColor = Color.FromArgb(120, 120, 120);
                pcsx2Running = false;
            }
        }

        private void tmr_GameData_Tick(object sender, EventArgs e)
        {
            if(pcsx2Running)
            {
                m.OpenProcess(PCSX2PROCESSNAME + ".exe");

                //Check to make sure we are in an Area server.
                if (m.readByte(GameHelper.CONNECTED_TO_AS_ADDRESS) == 1 )
                {
                    //Get party count
                    //Not really useful right now since I can't seem to get this to display
                    int partyCount = m.readByte(GameHelper.PARTY_COUNT_ADDRESS);
                    
                    //Get the server name
                    string serverName = ByteConverstionHelper.converyBytesToSJIS(m.readBytes(GameHelper.SERVER_NAME_ADDRESS, 20));
                    
                    //Get the town you are in.
                    //This address actually shows the current zone ccs file. So when you zone into an area this will update to that zone's CCS file
                    //Could be useful to display the area you are in but using the default assets wouldn't be telling of where the player actually is.
                    string townFileName = ByteConverstionHelper.convertBytesToString(m.readBytes(GameHelper.ZONE_CCS_FILE_LOADED, 12)).Split('.')[0].ToLower();

                    //Don't want the timer to continue if the player is just sitting in town.
                    if(townFileName.ToLower().Contains("town"))
                    {
                        presence.Timestamps = new Timestamps()
                        {
                           Start = null

                        };
                        setPresence(serverName, "", "", "", partyCount, townFileName);
                        gameStarted = false;
                    }
                   
                    //Check to see if we are in a zone. (Could technically use townFileName, but I haven't don't extensive testing with that to see if it is
                    // always a zone file name that is loaded there.)
                    if (m.readByte(GameHelper.IN_AREA_ADDRESS) == 1)
                    {
                        //Get the pointer addresses and add their offsets to them
                        int areaword1Pointer = int.Parse(ByteConverstionHelper.byteArrayHexToAddressString(m.readBytes(GameHelper.AREA_WORD_POINTER, 4)), System.Globalization.NumberStyles.HexNumber) + GameHelper.AREA_WORD_ONE_POINTER_OFFSET;
                        int areaWord2Pointer = int.Parse(ByteConverstionHelper.byteArrayHexToAddressString(m.readBytes(GameHelper.AREA_WORD_POINTER, 4)), System.Globalization.NumberStyles.HexNumber) + GameHelper.AREA_WORD_TWO_POINTER_OFFSET;
                        int areaWord3Pointer = int.Parse(ByteConverstionHelper.byteArrayHexToAddressString(m.readBytes(GameHelper.AREA_WORD_POINTER, 4)), System.Globalization.NumberStyles.HexNumber) + GameHelper.AREA_WORD_THREE_POINTER_OFFSET;

                        //Convert them to the useable address (ex. 20000000)
                        string areaWord1PointerAddress = ByteConverstionHelper.byteArrayHexToAddressString(m.readBytes(areaword1Pointer.ToString("X4"), 4));
                        string areaWord2PointerAddress = ByteConverstionHelper.byteArrayHexToAddressString(m.readBytes(areaWord2Pointer.ToString("X4"), 4));
                        string areaWord3PointerAddress = ByteConverstionHelper.byteArrayHexToAddressString(m.readBytes(areaWord3Pointer.ToString("X4"), 4));

                        //Get the keywords.
                        string areaWord1 = ByteConverstionHelper.converyBytesToSJIS(m.readBytes(areaWord1PointerAddress, 12)).Substring(4);
                        string areaWord2 = ByteConverstionHelper.converyBytesToSJIS(m.readBytes(areaWord2PointerAddress, 12)).Substring(4);
                        string areaWord3 = ByteConverstionHelper.converyBytesToSJIS(m.readBytes(areaWord3PointerAddress, 12)).Substring(4);

                        //Check to see if the game is started (if the player is in a zone) so the timer won't constantly reset
                        if(!gameStarted)
                        {
                            presence.Timestamps = new Timestamps()
                            {
                                Start = DateTime.UtcNow

                            };

                            gameStarted = true;
                        }
                        //Set their discord presence
                        setPresence(serverName, areaWord1, areaWord2, areaWord3, partyCount, "default");

                    }

                }
                else
                {
                    setPresence("", "", "", "", 0, "default");
                }
            }
        }
        private void setPresence(string serverName, string word1, string word2, string word3, int playerCount,string zone)
        {
            //Only checking the first word. If this isn't set then it's save to assume that we weren't in a zone
            if(word1 =="")
            {
                presence.Details = "Not currently in an area.";
            }
            else
            {
                presence.Details = "Area: " + word1 + " " + word2 + " " + word3;
            }
           
            if(serverName == "")
            {
                presence.State = "Not currently in a server.";
            }
            else
            {
                presence.State = "Area Server: " + serverName;
            }

            presence.Assets = new Assets();
      
            presence.Assets.LargeImageKey = zone;
            presence.Assets.SmallImageKey = zone;         

            client.SetPresence(presence);
        }

        private void frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Dispose();
        }
    }
}
