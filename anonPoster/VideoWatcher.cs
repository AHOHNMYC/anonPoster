using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace anonPoster {
    public class VideoWatcher {
#if DEBUG
        private const int STREAM_TIMER_RESOLUTION = 10000;
#else
        private const int STREAM_TIMER_RESOLUTION = 42000;
#endif

        bool isStream = false;
        Timer streamTestTimer;

        WebClient videoWC = new WebClient();

        private MainForm mf;

        public VideoWatcher(MainForm _mf) {
            mf = _mf;

            // Ignore all certificate checks for cybergame.tv
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                chain.ChainStatus[0].Status == X509ChainStatusFlags.NoError
                | ((HttpWebRequest)sender).Address.Authority.EndsWith(".cybergame.tv");

            streamTestTimer = new Timer();
            streamTestTimer.Tick += (sender, e) => TestStream();
            streamTestTimer.Interval = STREAM_TIMER_RESOLUTION;
            streamTestTimer.Enabled = Properties.Settings.Default.watchVidimo;
            // Check once immediately after run
            if (Properties.Settings.Default.watchVidimo)
                TestStream();
        }

        public bool ToggleStreamTestTimer() {
            streamTestTimer.Enabled = !streamTestTimer.Enabled;
            if (streamTestTimer.Enabled)
                TestStream();

            return streamTestTimer.Enabled;
        }


        void TestStream() {
            streamTestTimer.Stop();
            bool isStreamNow = false;

            try {
                string response = videoWC.DownloadString(URLs.mainStreamInfo);
                // Chech of JSON answer contains online":"1
                isStreamNow = response.IndexOf("online\":\"1") != -1;
            } catch (Exception e) {
                mf.HandleException(e);
            }

            if (isStreamNow && !isStream) {
                string question = @"    Н͔͊а͇̽ч̮̽а͇͘л̱̊а̱̏с͍͠ь͉̔ ̹̊т̢̓р͇͌а̤͆н̥̂с̞͑л͓̂я̨̿ц̗̀и̥̍я̻̆.̢͐
                      ҉̕͞
̵̢̨Т̣̾ы͓̿ ̡̏х̮̕о̺͆ч͍̔е̘͌ш̗͑ь̖̒ ̮͠О͉̒ ͖͠Т̭͛ ̕ͅК͈̍ ̣̏Р̥͠ ̳͛Ы̨̉ ̞̉Т̖̓ ͇͌Ь̡̀ ̩̈́е̝̒ё̟̾ ?̪̕";

                mf.Visible = false;
                if (mf.streamsForm != null)
                    mf.streamsForm.Visible = false;

                if (DialogResult.Yes == MessageBox.Show(question, "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
                    mf.ShowStreamsForm();

            }
            if (!isStreamNow && isStream) {
                mf.TrayIcon.ShowBalloonTip(1000, null, "Стрим закончился", ToolTipIcon.Warning);
            }
            isStream = isStreamNow;
            streamTestTimer.Start();
        }
    }
}
