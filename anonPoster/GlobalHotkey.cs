using System;
using System.Windows.Forms;

namespace anonPoster {
    // https://www.dreamincode.net/forums/topic/180436-global-hotkeys/
    class GlobalHotkey {
        public static class Constants {
            //modifiers
            public const int NOMOD = 0x0000;
            public const int ALT = 0x0001;
            public const int CTRL = 0x0002;
            public const int SHIFT = 0x0004;
            public const int WIN = 0x0008;

            //windows message id for hotkey
            public const int WM_HOTKEY_MSG_ID = 0x0312;
        }

        private int modifier;
        private int key;
        private IntPtr hWnd;
        private int id {
            get { return GetHashCode(); }
        }

        public GlobalHotkey(int _modifier, Keys _key, Form _form) {
            modifier = _modifier;
            key = (int)_key;
            hWnd = _form.Handle;
        }

        public override int GetHashCode() {
            return modifier ^ key ^ hWnd.ToInt32();
        }

        public bool Register() {
            return NativeMethods.RegisterHotKey(hWnd, id, modifier, key);
        }
        public bool Unregiser() {
            return NativeMethods.UnregisterHotKey(hWnd, id);
        }
    }
}
