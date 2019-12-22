using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fragmentDiscordPresence
{
    public static class GameHelper
    {
        public const string CONNECTED_TO_AS_ADDRESS = "206F92F0";
        public const string AREA_WORD_POINTER = "206F9438";
        public const string SERVER_NAME_ADDRESS = "206F94F2";
        public const string PARTY_COUNT_ADDRESS = "208B93A8";
        public const string IN_AREA_ADDRESS = "20877DA8"; 
        public const string ZONE_CCS_FILE_LOADED = "206541F4";
        public const int AREA_WORD_ONE_POINTER_OFFSET= 4;
        public const int AREA_WORD_TWO_POINTER_OFFSET = 8;
        public const int AREA_WORD_THREE_POINTER_OFFSET = 12;
    }
}
