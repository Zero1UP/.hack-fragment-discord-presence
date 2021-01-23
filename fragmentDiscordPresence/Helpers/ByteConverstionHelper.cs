using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fragmentDiscordPresence
{
    public static class ByteConverstionHelper
    {
        public static string convertBytesToString(byte[] data)
        {
            if (data != null)
            {
                string result;

                data.Reverse();

                result = Encoding.Default.GetString(data);

                var cleanedString = result.Split(new string[] { "\0" }, StringSplitOptions.None);
                return cleanedString[0].Replace(',', ' ');
            }
            return "";
        }

        /// <summary>
        /// Takes the passed in byte array and converts it to be used as the next lookup address. This is mainly used for when dealing
        /// with pointer values. ex: 200A0000
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string byteArrayHexToAddressString(byte[] data, bool replaceFirstDigit = true)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte b in data.Reverse())
            {
                result.AppendFormat("{0:x2}", b);
            }
            if (replaceFirstDigit)
            {
                result[0] = '2';
            }

            return result.ToString();
        }

        public static string converyBytesToSJIS(byte[] data)
        {
            try
            {
                var cleanedString = Encoding.GetEncoding(932).GetString(data).Split(new string[] { "\0" }, StringSplitOptions.None);
                return cleanedString[0].Replace(',', ' ');
            }
            catch (Exception)
            {

                return "";
            }


        }
    }
}
