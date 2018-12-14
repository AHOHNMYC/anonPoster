using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace anonPoster {
    public class RadioWatcher {
#if DEBUG
        private const int RADIO_TIMER_RESOLUTION = 10000;
#else
        private const int RADIO_TIMER_RESOLUTION = 42000;
#endif

        private Timer radioTestTimer;
        private WebClient radioWC = new WebClient();
        private bool wasLive;
        private int lastCover;

        private MainForm mf;

        public RadioWatcher(MainForm _mf) {
            mf = _mf;

            radioTestTimer = new Timer();
            radioTestTimer.Tick += (sender, e) => TestRadio();
            radioTestTimer.Interval = RADIO_TIMER_RESOLUTION;
            radioTestTimer.Enabled = Properties.Settings.Default.watchRadio;
            // Check once immediately after run
            if (Properties.Settings.Default.watchRadio)
                TestRadio();
        }


        public bool ToggleRadioTestTimer() {
            radioTestTimer.Enabled = !radioTestTimer.Enabled;
            if (radioTestTimer.Enabled)
                TestRadio();

            return radioTestTimer.Enabled;
        }



        public void TestRadio() {
            radioTestTimer.Stop();

            try {
                byte[] pageBytes = radioWC.DownloadData(URLs.radioState);
                string pageText = Encoding.UTF8.GetString(pageBytes);

                string[] delimited = pageText.Split('\n');
                Dictionary<string, string> d = new Dictionary<string, string>();
                for (int i = 0; delimited.Length - i >= 2; i += 2)
                    d.Add(delimited[i], delimited[i + 1]);

                if (d.ContainsKey("cover")) {
                    int newCover = int.Parse(d["cover"]);
                    if (newCover != lastCover) {
                        lastCover = newCover;
                        mf.CoverBox.LoadAsync(URLs.RadioCover(newCover));
                    }
                }

                StringBuilder trackSb = new StringBuilder();
                if (d.ContainsKey("Artist"))
                    trackSb.Append(d["Artist"]);
                if (d.ContainsKey("Title"))
                    trackSb.Append($" — {d["Title"]}");
                string track = trackSb.ToString();

#if DEBUG
                Debugger.Log(5, "Получили имя трека", track + "\n");
#endif

                mf.coverTt.SetToolTip(mf.CoverBox, track);

                if (Properties.Settings.Default.warnAboutLive && d.ContainsKey("isLive")) {
                    bool isLive = int.Parse(d["isLive"]) == 1;
                    if (isLive && !wasLive) {
                        mf.streamsForm?.Close();
                        MessageBox.Show(mf, "Началось живое вещщание\nПодключайся");
                    }
                    wasLive = isLive;
                }

                if (track.Length > 0) {
                    if (mf.ltm.UpdateCoverMenu(DateTime.Now, track)) {

                        if (Properties.Settings.Default.warnAboutTrackChange)
                            mf.TrayIcon.ShowBalloonTip(0, null, track, ToolTipIcon.Info);

                        mf.CoverBox.ContextMenu = mf.ltm.menu;
                    }
                }

            } catch (WebException e) when (e.Status is WebExceptionStatus.ProtocolError && e.Response is HttpWebResponse r) {
                // If we have 404, it means stream hasn't been started
                // All is OK. Just skip it
                if (r.StatusCode != HttpStatusCode.NotFound)
                    mf.HandleException(e);
            } catch (Exception e) {
                mf.HandleException(e);
            }

            radioTestTimer.Start();
        }

    }
}
