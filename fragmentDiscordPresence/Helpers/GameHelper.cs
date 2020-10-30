using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fragmentDiscordPresence
{
    public static class GameHelper
    {
        public static IntPtr CONNECTED_TO_AS_ADDRESS = new IntPtr(0x206F92F0);
        public static IntPtr AREA_WORD_POINTER = new IntPtr(0x206F9438);
        public static IntPtr SERVER_NAME_ADDRESS =new IntPtr(0x206F94F2);
        public static IntPtr PARTY_COUNT_ADDRESS = new IntPtr(0x208B93A8);
        public static IntPtr IN_AREA_ADDRESS = new IntPtr(0x20877DA8); 
        public static IntPtr ZONE_CCS_FILE_LOADED = new IntPtr(0x206541F4);
        public const int AREA_WORD_ONE_POINTER_OFFSET= 4;
        public const int AREA_WORD_TWO_POINTER_OFFSET = 8;
        public const int AREA_WORD_THREE_POINTER_OFFSET = 12;
    }
}
