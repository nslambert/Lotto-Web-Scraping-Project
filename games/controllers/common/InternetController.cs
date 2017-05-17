using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;

namespace ttpim.gamemodule.games.controllers.common
{
    public static class InternetController
    {
        private static readonly int INTERNET_STATE_DISCONNECTED = 16;
        private static readonly int INTERNET_STATE_CONNECTED = 1;
        private static readonly int ISO_FORCE_DISCONNECTED = 1;
        private static readonly int INTERNET_OPTION_CONNECTED_STATE = 50;

        [StructLayout(LayoutKind.Sequential)]
        struct INTERNET_CONNECTED_INFO
        {
            public int dwConnectedState;
            public int dwFlags;
        };

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        [DllImport("wininet.dll")]
        private extern static bool InternetSetOption(int hInternet, int dwOption, ref INTERNET_CONNECTED_INFO lpBuffer, int dwBufferLength);

        private static void SetIEConnectionMode(bool connectionMode)
        {
            INTERNET_CONNECTED_INFO ici = new INTERNET_CONNECTED_INFO();

            if (connectionMode)
            {
                ici.dwConnectedState = INTERNET_STATE_CONNECTED;
            }
            else
            {
                ici.dwConnectedState = INTERNET_STATE_DISCONNECTED;
                ici.dwFlags = ISO_FORCE_DISCONNECTED;
            }

            InternetSetOption(0, INTERNET_OPTION_CONNECTED_STATE, ref ici, Marshal.SizeOf(ici));
        }

        private static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static bool GetConnectionStatus()
        {
            if (IsConnectedToInternet() == true)
            {
                ValidateWebBrowserConnectionMode();
                return true;
            }
            else
                return false;
        }

        public static void ValidateWebBrowserConnectionMode()
        {
            if (IsConnectedToInternet())
            {
                SetIEConnectionMode(true);
            }

        }
    }
}
