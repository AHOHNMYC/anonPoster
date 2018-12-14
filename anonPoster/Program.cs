using System;
using System.Windows.Forms;

namespace anonPoster {
    static class Program {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main() {
            AnalyzeCmdLine();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());
        }

        public static bool startInBackground = false;

        static void AnalyzeCmdLine() {
            foreach (string s in Environment.GetCommandLineArgs())
                switch (s) {
                    case "/bg":
                        startInBackground = true;
                        break;
                    default: break;
                }
        }
    }
}
