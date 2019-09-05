using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace anonPoster {
    public class ScheduleWatcher {
#if DEBUG
        private const int SCHED_TIMER_RESOLUTION = 10000;
#else
        private const int SCHED_TIMER_RESOLUTION = 42000;
#endif

        private Timer schedTestTimer;
        private WebClient schedWC = new WebClient();
        private int lastHash = 0;

        private MainForm mf;

        public Event[] events;

        public ScheduleWatcher(MainForm _mf) {
            mf = _mf;

            schedTestTimer = new Timer();
            schedTestTimer.Tick += (sender, e) => TestRadio();
            schedTestTimer.Interval = SCHED_TIMER_RESOLUTION;
            schedTestTimer.Enabled = Properties.Settings.Default.watchSched;
            // Check once immediately after run
            if (Properties.Settings.Default.watchSched)
                TestRadio();
        }

        public bool ToggleSchedTestTimer() {
            schedTestTimer.Enabled = !schedTestTimer.Enabled;
            if (schedTestTimer.Enabled)
                TestRadio();

            return schedTestTimer.Enabled;
        }



        public void TestRadio() {
            schedTestTimer.Stop();

            try {
                byte[] pageBytes = schedWC.DownloadData(URLs.radioSched);
                string pageText = Encoding.UTF8.GetString(pageBytes);

                if (pageText.GetHashCode() == lastHash)
                    return;
                lastHash = pageText.GetHashCode();

                events = ParseJs(pageText);
#if DEBUG
/*
                Debugger.Log(5, "", "Получили расписание:\n");
                foreach (Event e in events)
                    Debugger.Log(5, "", FormatEventString(e));
*/
#endif
                mf.scheduleForm.ClearSchedule();
                foreach (Event e in events) {
                    mf.scheduleForm.AddEvent(FormatEventString(e));

                    if (DateTime.Now > e.Start && DateTime.Now < e.End)
                        MessageBox.Show(mf, FormatEventString(e), "Прозвенел третий звонок!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
#if DEBUG
                mf.scheduleForm.Show(mf);
#endif
            } catch (WebException e) when (e.Status == WebExceptionStatus.ProtocolError && e.Response is HttpWebResponse) {
                // If we have 404, it means stream hasn't been started
                // All is OK. Just skip it
                HttpWebResponse r = e.Response as HttpWebResponse;
                if (r.StatusCode != HttpStatusCode.NotFound)
                    mf.HandleException(e);
            } catch (Exception e) {
                mf.HandleException(e);
            }

            schedTestTimer.Start();
        }


        private string FormatEventString(Event e) {
            return $"{FormatRemainded(e.Start, e.End)}. Длительность: {e.End - e.Start} Ведущий: {e.Host}, \"{e.Title}\"\n";
        }


        public class Event {
            public DateTime Start;
            public DateTime End;
            public string Host;
            public string Title;
        }

        // epoch -> DateTime in current time zone
        private DateTime ConvertSeconds(int seconds) => TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1).AddSeconds(seconds));

        Event[] ParseJs(string js) {
            List<Event> evs = new List<Event>();

            js = js.Substring(2, js.Length - 2);
            string[] events = js.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ev in events) {
                string cleaned = ev.Substring(1, ev.Length - 2); // Remove "
                string[] tokens = cleaned.Split(new string[] { "\",\""}, StringSplitOptions.RemoveEmptyEntries);
                evs.Add(new Event {
                    Start = ConvertSeconds(int.Parse(tokens[0])),
                    End   = ConvertSeconds(int.Parse(tokens[1])),
                    Host  = tokens[2].Trim(),
                    Title = tokens[3].Trim(),
                });
            }
            return evs.ToArray();
        }

        string FormatRemainded(DateTime s, DateTime e) {

            if (DateTime.Now > e)
                return "Передача уже прошла";

            string prefix;
            DateTime pointOfNoReturn;

            if (DateTime.Now > s) {
                prefix = "Ещё";
                pointOfNoReturn = e;
            } else {
                prefix = "Через";
                pointOfNoReturn = s;
            }

            DateTime n = DateTime.Now;
            //int deltaSeconds = pointOfNoReturn.Second - n.Second;
            int deltaMinutes = pointOfNoReturn.Minute - n.Minute;
            int deltaHours = pointOfNoReturn.Hour - n.Hour;
            int deltaDays = pointOfNoReturn.Day - n.Day;
            int deltaMonths = pointOfNoReturn.Month - n.Month;
            int deltaYears = pointOfNoReturn.Year - n.Year;
            int deltaCenturies = deltaYears/100;
            int deltaMillenniums = deltaCenturies/10;

            // If unit is zero, we breaks into next switch
            // Until we reach non-zero unit, that will return some value
            switch (deltaMillenniums) {
                case 0: break;
                case 1: return $"{prefix} 1 тысячелетие";
                case 2: case 3: case 4: return $"{prefix} {deltaMillenniums} тысячелетия";
                default: return $"{prefix} {deltaMillenniums} тысячелетий";
            }
            switch (deltaCenturies) {
                case 0: break;
                case 1: return $"{prefix} 1 век";
                case 2: case 3: case 4: return $"{prefix} {deltaCenturies} века";
                default: return $"{prefix} {deltaCenturies} веков";
            }
            switch (deltaYears) {
                case 0: break;
                case 1: return $"{prefix} 1 год";
                case 2: case 3: case 4: return $"{prefix} {deltaYears} года";
                default: return $"{prefix} {deltaYears} лет";
            }
            switch (deltaMonths) {
                case 0: break;
                case 1: return $"{prefix} 1 месяц";
                case 2: case 3: case 4: return $"{prefix} {deltaMonths} месяца";
                default: return $"{prefix} {deltaMonths} месяцев";
            }
            switch (deltaDays) {
                case 0: break;
                case 1: return $"{prefix} 1 день";
                case 2: case 3: case 4: return $"{prefix} {deltaDays} дня";
                default: return $"{prefix} {deltaDays} дней";
            }
            switch (deltaHours) {
                case 0: break;
                case 1: return $"{prefix} 1 час";
                case 2: case 3: case 4: return $"{prefix} {deltaHours} часа";
                default: return $"{prefix} {deltaHours} часов";
            }
            switch (deltaMinutes) {
                case 0: case 1: return $"{prefix} 1 минута";
                case 2: case 3: case 4: return $"{prefix} {deltaMinutes} минуты";
                default: return $"{prefix} {deltaMinutes} минут";
            }
        }
    }
}
