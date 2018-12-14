using System;
using System.Runtime.InteropServices;

namespace anonPoster {
    static class NativeMethods {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        public const int HTBOTTOMRIGHT = 0x11;

        [DllImport("User32")]
        public static extern bool ReleaseCapture();
        [DllImport("User32")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("User32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
