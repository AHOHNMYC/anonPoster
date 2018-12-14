using System.Reflection;
using Microsoft.Win32;

namespace anonPoster {
    static class Autostart {

        private static RegistryKey run = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        private static string GetAssemblyName() {
            string s = Assembly.GetEntryAssembly().FullName;
            return s.Substring(0, s.IndexOf(","));
        }


        /// <summary>
        /// Checks if that assembly is added to user's auto start list
        /// </summary>
        /// <returns>State of autostart of this assembly</returns>
        public static bool Check() {
            return run.GetValue(GetAssemblyName(), null) != null;
        }

        /// <summary>
        /// Adds or deletes assembly from user's auto start list
        /// </summary>
        /// <param name="newState">true - enable autostart, false - disable autostart</param>
        /// <returns>State of autostart of this assembly</returns>
        public static bool Set(bool newState) {
            if (newState)
                run.SetValue(GetAssemblyName(), Assembly.GetEntryAssembly().Location);
            else 
                run.DeleteValue(GetAssemblyName(), false);

            return Check();
        }
    }
}
