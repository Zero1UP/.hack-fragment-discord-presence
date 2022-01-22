using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement.Native;
using Microsoft.Win32.SafeHandles;

namespace fragmentDiscordPresence
{
    public static class GameHelper
    {
        public const long PCSX2_32BIT_ADDRESSPTR = 0x20000000;
        public const long PCSX2_64BIT_ADDRESSPTR = 0x7FF750000000;
        
        public const int CONNECTED_TO_AS_ADDRESS = 0x6F92F0;
        public const int AREA_WORD_POINTER = 0x6F9438;
        public const int SERVER_NAME_ADDRESS =0x6F94F2;
        public const int PARTY_COUNT_ADDRESS = 0x8B93A8;
        public const int IN_AREA_ADDRESS = 0x877DA8; 
        public const int ZONE_CCS_FILE_LOADED = 0x6541F4;
        public const int AREA_WORD_ONE_POINTER_OFFSET= 4;
        public const int AREA_WORD_TWO_POINTER_OFFSET = 8;
        public const int AREA_WORD_THREE_POINTER_OFFSET = 12;
        
        
        /// <summary>
        /// Get the Offset of the PCSX2 Emulator based whether it's running the 32bit version or 64bit
        /// </summary>
        /// <param name="process"></param>
        /// <returns>PCSX2 Offset</returns>
        public static long GetPcsx2Offset(Process process)
        {
            bool isWow64 = true;
            
            if (Environment.Is64BitOperatingSystem)
            {
                SafeProcessHandle processHandle = process.SafeHandle;
                NativeMethods.IsWow64Process(processHandle, out isWow64);
            }

            if (isWow64)
            {
                return PCSX2_32BIT_ADDRESSPTR;
            }
            else
            {
                return PCSX2_64BIT_ADDRESSPTR;
            }


        }
    }
}
