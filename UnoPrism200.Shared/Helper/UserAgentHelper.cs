using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UnoPrism200.Helpers
{
    public class UserAgentHelper
    {
        private const int URLMON_OPTION_USERAGENT = 0x10000001;

        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength,
            int dwReserved);

        /// <summary>
        /// SetUserAgent
        /// </summary>
        public static void SetDefaultUserAgent(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return;
            }

            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, userAgent, userAgent.Length, 0);
        }
    }
}
