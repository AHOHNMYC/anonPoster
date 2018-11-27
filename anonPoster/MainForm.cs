using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace anonPoster {
    public partial class MainForm : Form {
        // Constatns
#if DEBUG
        private const int STREAM_TIMER_RESOLUTION = 10000;
#else
        private const int STREAM_TIMER_RESOLUTION = 42000;
#endif
        private const int RADIO_TIMER_RESOLUTION = STREAM_TIMER_RESOLUTION;

        // Global variables
        int CaptchaID;  // updates by ReloadCaptcha()
        Streams streamsForm;
        Image captchaOrig;
        bool isStream = false;
        Timer streamTestTimer;
        Timer radioTestTimer;
        Random mainRandom = new Random();
        int lastCover;
        ToolTip coverTt = new ToolTip { IsBalloon = true };
        bool wasLive;

        void Recolor(Color backColorIn, Color foreColorIn) {
            Properties.Settings.Default.foreColor = foreColorIn;
            Properties.Settings.Default.backColor = backColorIn;
            Properties.Settings.Default.Save();
            Recolor();
        }
        void Recolor() {
            ForeColor = KukarekBox.ForeColor = CaptchaAnswer.ForeColor = Properties.Settings.Default.foreColor;
            BackColor = KukarekBox.BackColor = CaptchaAnswer.BackColor = Properties.Settings.Default.backColor;
            ColorizeCaptcha();
        }


        public MainForm() {
            InitializeComponent();
        }

        void GlobHotkey() {
            new GlobalHotkey(GlobalHotkey.Constants.CTRL + GlobalHotkey.Constants.SHIFT, Keys.K, this).Register();
        }

        void ToggleMenuRadiobutton(object s) {
            foreach (MenuItem m in ((MenuItem)s).Parent.MenuItems)
                m.Checked = false;
            ((MenuItem)s).Checked = true;
        }

        void ShowStreamsForm() {
            if (streamsForm == null) {
                streamsForm = new Streams(this) { Font = Font, Icon = Icon };
            }
            streamsForm.TopMost = TopMost;
            streamsForm.ShowDialog();
        }

        private void OnMenuChangeColor(object s, Color back, Color fore) {
            Recolor(back, fore);
            ToggleMenuRadiobutton(s);
        }

        void MakeTray() {
            TrayIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Радио Анонимус =>", (s, e) => Process.Start(URLs.radio)),
                new MenuItem("Видимо", (s, e) => ShowStreamsForm()),
                new MenuItem("-"),
                new MenuItem("Подмять говно", (s, e) => {
                    Properties.Settings.Default.topMost = ((MenuItem)s).Checked = TopMost = !TopMost;
                    Properties.Settings.Default.Save();
                }) { Checked = Properties.Settings.Default.topMost },
                new MenuItem("Xzibit", new MenuItem[] {
                    new MenuItem("Побелеть", (s, e) => OnMenuChangeColor(s, Color.White, Color.Black)) { RadioCheck = true },
                    new MenuItem("Почернеть", (s, e) => OnMenuChangeColor(s, Color.Black, Color.White)) { RadioCheck = true },
                    new MenuItem("Шиндовс", (s, e) => OnMenuChangeColor(s, DefaultBackColor, DefaultForeColor)) { RadioCheck = true },
                    new MenuItem("Каштом", (s, e) => {
                        ColorDialog colorPick = new ColorDialog{ FullOpen = true };

                        Color b = colorPick.Color = BackColor;
                        if (colorPick.ShowDialog() == DialogResult.OK)
                            b = colorPick.Color;

                        Color f = colorPick.Color = ForeColor;
                        if (colorPick.ShowDialog() == DialogResult.OK)
                            f = colorPick.Color;

                        OnMenuChangeColor(s, b, f);
                    }) { RadioCheck = true, Checked = true },
                }),
                /*
                new MenuItem("Я мышеблядь, покажи окно снизу~~", (s, e) => {
                    ((MenuItem)s).Checked = ShowInTaskbar = !ShowInTaskbar;
                    GlobHotkey();
                }),
                */
                new MenuItem("Следить за видимом", (s, e) => {
                    if (Properties.Settings.Default.watchVidimo = ((MenuItem)s).Checked = streamTestTimer.Enabled = !streamTestTimer.Enabled)
                        TestStream();
                    Properties.Settings.Default.Save();
                }) { Checked = Properties.Settings.Default.watchVidimo },
                new MenuItem("Следить за радивой", new MenuItem[] {
                    new MenuItem("Следить за радивой", (s, e) => {
                        if (Properties.Settings.Default.watchRadio = ((MenuItem)s).Checked = radioTestTimer.Enabled = !radioTestTimer.Enabled)
                            TestRadio();
                        Properties.Settings.Default.Save();
                    }) { Checked = Properties.Settings.Default.watchRadio },
                    new MenuItem("Предупреждать о смене трека", (s, e) => {
                        ((MenuItem)s).Checked = Properties.Settings.Default.warnAboutTrackChange = !Properties.Settings.Default.warnAboutTrackChange;
                        Properties.Settings.Default.Save();
                    }) { Checked = Properties.Settings.Default.warnAboutTrackChange },
                    new MenuItem("Предупреждать о начале живого вещания", (s, e) => {
                        ((MenuItem)s).Checked = Properties.Settings.Default.warnAboutLive = !Properties.Settings.Default.warnAboutLive;
                        Properties.Settings.Default.Save();
                    }) { Checked = Properties.Settings.Default.warnAboutLive },
                }),
                new MenuItem("Обход капчи (эксперим.)", (s, e) => {
                    ((MenuItem)s).Checked = Properties.Settings.Default.useGodGrace = !Properties.Settings.Default.useGodGrace;
                    Properties.Settings.Default.Save();
                    GodGraceVisualUpdate();
                }) { Checked = Properties.Settings.Default.useGodGrace },
                new MenuItem("-"),
                new MenuItem("Что умеет?", (s, e) => HelpMeee()),
                new MenuItem("Выход", (s, e) => Close())
            });
        }

        void GodGraceVisualUpdate() {
            const byte captchaDelta = 12;

            if (Properties.Settings.Default.useGodGrace) {
                CaptchaAnswer.Hide();
                CaptchaPicture.Cursor = Cursors.SizeAll;
                CaptchaPicture.MouseDown += DragByMouse;
                CaptchaPicture.Top += captchaDelta;

                IdLabel.BringToFront();

                CaptchaPicture.Image = Properties.Resources.graceOfGod;
                captchaOrig = (Image)CaptchaPicture.Image.Clone();
                ColorizeCaptcha();
            } else {
                CaptchaAnswer.Show();
                CaptchaPicture.Cursor = Cursors.Hand;
                CaptchaPicture.MouseDown -= DragByMouse;
                CaptchaPicture.Top -= captchaDelta;

                CaptchaPicture.BringToFront();
                LeftSymbolsLabel.BringToFront();
                StatusLabel.BringToFront();

                CaptchaPicture.Image = null;
                ReloadCaptcha();
            }
        }

        void ColorizeCaptcha() {
            if (null == CaptchaPicture.Image)
                return;

            ColorPalette p = captchaOrig.Palette;
            for (int i = 0; i < p.Entries.Length; i++) {
                float k1 = (float)p.Entries[i].R / 256;
                float k2 = (byte.MaxValue - (float)p.Entries[i].R) / 256;

                p.Entries[i] = Color.FromArgb(
                    (int)(k1 * BackColor.R + k2 * ForeColor.R),
                    (int)(k1 * BackColor.G + k2 * ForeColor.G),
                    (int)(k1 * BackColor.B + k2 * ForeColor.B)
                    );
            }
            CaptchaPicture.Image.Palette = p;
        }

        void ReloadCaptcha(string PageText = null) {
#if DEBUG
            Debugger.Log(5, "хуй", "Reloading captcha\n");
#endif
            try {
                if (PageText == null) {
                    byte[] pageBytes = new WebClient().DownloadData(URLs.radioFeedback);
                    PageText = Encoding.UTF8.GetString(pageBytes);
                }

                CaptchaID = int.Parse(PageText.Substring(1499, 6));
                ChangeCaptchaImage(URLs.RadioCaptcha(CaptchaID));
            } catch (WebException e) {
                switch (e.Status) {
                    case WebExceptionStatus.ConnectFailure:
                        ChangeCaptchaImage(Properties.Resources.captchaNoInternet);
                        break;
                    case WebExceptionStatus.ProtocolError:
                        MessageBox.Show("Охуеть. Капча пропала", "Ёбаный пиздец", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        break;
                    default:
                        HandleException(e);
                        break;
                }
            } catch (Exception e) {
                HandleException(e);
            } finally {
                CaptchaAnswer.Text = null;
                CaptchaAnswer.Focus();
            }
        }

        void ChangeCaptchaImage(string url) {
            CaptchaPicture.Load(url);
            _ChangeCaptchaImage();
        }
        void ChangeCaptchaImage(Image newImage) {
            CaptchaPicture.Image = (Image)newImage.Clone();
            _ChangeCaptchaImage();
        }
        void _ChangeCaptchaImage() {
            captchaOrig = (Image)CaptchaPicture.Image.Clone();
            ColorizeCaptcha();
        }

        void HandleException(Exception e) {
            if (e is WebException we && we.Status == WebExceptionStatus.ConnectFailure)
                ChangeCaptchaImage(Properties.Resources.captchaNoInternet);
            else
                TrayIcon.ShowBalloonTip(1000, null, e.StackTrace + '\n' + e.Message, ToolTipIcon.Warning);
        }


        void DragByMouse(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HTCAPTION, 0);
            }
        }

        
        void ResizeByMouse(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HTBOTTOMRIGHT, 0);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m) {
            // Ctrl+Shift+K handler
            if (m.Msg == GlobalHotkey.Constants.WM_HOTKEY_MSG_ID) {
                //WindowState = FormWindowState.Normal;
                if (streamsForm == null || !streamsForm.Visible) {
                    Visible = true;
                    BringToFront();
                    Activate();
                } else {
                    streamsForm.BringToFront();
                    streamsForm.Activate();
                }
            }
            base.WndProc(ref m);
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData) {
            switch (keyData) {
                case Keys.Escape:
                    //WindowState = FormWindowState.Minimized;
                    Visible = false;
                    return true;
                case Keys.F1:
                    HelpMeee();
                    return true;
                case Keys.Control | Keys.Enter:
                    Kukarek();
                    return true;
            }
            return base.ProcessDialogKey(keyData);
        }



        bool Kukarek() {
            int left = 500 - KukarekBox.Text.Length;
            if (left == 500) {
                TrayIcon.ShowBalloonTip(1000, null, "Введи что-нибудь", ToolTipIcon.Warning);
                KukarekBox.Focus();
                return false;
            }
            if (left < 0) {
                TrayIcon.ShowBalloonTip(1000, null, "Превышен лимит в 500 символов", ToolTipIcon.Warning);
                KukarekBox.Focus();
                return false;
            }

            if (!Properties.Settings.Default.useGodGrace) {
                // Check captcha value. Have to be 5 digits
                if (CaptchaAnswer.Text.Length != 5) {
                    CaptchaAnswer.Focus();
                    return false;
                } else {
                    foreach (char c in CaptchaAnswer.Text) {
                        if (!char.IsDigit(c)) {
                            CaptchaAnswer.Focus();
                            return false;
                        }
                    }
                }
            }

            StringBuilder postDataSb = new StringBuilder();
            if (Properties.Settings.Default.useGodGrace) {
                postDataSb
                    .Append("cid=").Append(mainRandom.Next())
                    .Append("&msg=").Append(Uri.EscapeDataString(KukarekBox.Text))
                    .Append("&check=").Append(mainRandom.Next());
            } else {
                postDataSb
                    .Append("cid=").Append(CaptchaID)
                    .Append("&left=").Append(left)
                    .Append("&msg=").Append(Uri.EscapeDataString(KukarekBox.Text))
                    .Append("&check=").Append(CaptchaAnswer.Text);
            }

#if DEBUG
            Debugger.Log(5, "ALARM", postDataSb.ToString());
#endif

            Enabled = false;
            try {
                WebClient wc = new WebClient();
                if (Properties.Settings.Default.useGodGrace)
                    wc.Headers.Add(HttpRequestHeader.UserAgent, "AnonFM Player for Android");
                wc.Headers.Add(HttpRequestHeader.Referer, URLs.radioFeedback);
                string response = Encoding.UTF8.GetString(
                    wc.UploadData(URLs.radioFeedback, "POST", Encoding.ASCII.GetBytes(postDataSb.ToString()))
                    );

                if (response.IndexOf("Отправлено") > -1) {
                    // Extract userID
                    int idStart = response.IndexOf("strong") + 7;
                    int idLength = response.LastIndexOf("</strong") - idStart;
                    IdLabel.Text = response.Substring(idStart, idLength);

                    StatusLabel.Text = "OK";

                    if (!Properties.Settings.Default.useGodGrace)
                        ReloadCaptcha();

                    KukarekBox.Text = null;
                    KukarekBox.Focus();
                } else {
                    StatusLabel.Text = "ERR";

                    if (!Properties.Settings.Default.useGodGrace)
                        ReloadCaptcha(response);
                }
            } catch (Exception e) {
                HandleException(e);
            }

            return Enabled = true;
        }



        void HelpMeee() {
            //WindowState = FormWindowState.Normal;
            MessageBox.Show(@"Умеет радиачевать капчу и срать в кукарекалку

- Если подробно, то есть хоткеи:
Показывание главного окна по Ctrl+Shift+K
Сворачивание по Esc
Отправка сообщения по Ctrl+Enter
Отправка сообщения по Enter из поля ввода цифирь
Обновление капчи по R из поля ввода цифирь
Закрыть можно из меню в трее и по Alt+F4

* Если сообщение успешно отправилось, справа снизу будет показан твой ID, а статус будет ОК. Иначе, ERR

◙ Контекстное меню есть в трее и под полем ввода

♠ Через контекстное меню на обложке можно загуглить 10 последних треков

└ Софтина может предупреждать о запуске видимопотока", "Что умеет?", MessageBoxButtons.OK, MessageBoxIcon.Information);

            const string dev = "c0d3d by AHOHNMYC, 2o18\n\nПроверить обновления?";
            if (DialogResult.Yes == MessageBox.Show(dev, "d3ve1oler", MessageBoxButtons.YesNo, MessageBoxIcon.Information)) {
                Process.Start(URLs.selfUpdate);
            }
        }



        void SetDocking() {
            ResizePicture.Top = Height - ResizePicture.Height;
            ResizePicture.Left = Width - ResizePicture.Width;
            ResizePicture.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            KukarekBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            LeftSymbolsLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            StatusLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            IdLabel.Anchor = AnchorStyles.Right| AnchorStyles.Bottom;
            CaptchaPicture.Anchor = AnchorStyles.Bottom;
            CaptchaAnswer.Anchor = AnchorStyles.Bottom;
            CoverBox.Anchor = AnchorStyles.Bottom;
        }

        void TestStream() {
            streamTestTimer.Stop();
            bool isStreamNow = false;

            try {
                HttpWebRequest request = WebRequest.Create(URLs.mainStream) as HttpWebRequest;
                request.Method = "HEAD";
                request.GetResponse();
                isStreamNow = true;
            } catch (WebException e) when (e.Status is WebExceptionStatus.ProtocolError && e.Response is HttpWebResponse r) {
                // If we have 404, it means stream hasn't been started
                // All is OK. Just skip it
                if (r.StatusCode != HttpStatusCode.NotFound)
                    HandleException(e);
            } catch (Exception e) {
                HandleException(e);
            }

            if (isStreamNow && !isStream) {
                string question = @"    Н͔͊а͇̽ч̮̽а͇͘л̱̊а̱̏с͍͠ь͉̔ ̹̊т̢̓р͇͌а̤͆н̥̂с̞͑л͓̂я̨̿ц̗̀и̥̍я̻̆.̢͐
                      ҉̕͞
̵̢̨Т̣̾ы͓̿ ̡̏х̮̕о̺͆ч͍̔е̘͌ш̗͑ь̖̒ ̮͠О͉̒ ͖͠Т̭͛ ̕ͅК͈̍ ̣̏Р̥͠ ̳͛Ы̨̉ ̞̉Т̖̓ ͇͌Ь̡̀ ̩̈́е̝̒ё̟̾ ?̪̕";

                Visible = false;
                if (DialogResult.Yes == MessageBox.Show(question, "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
                    ShowStreamsForm();
                Visible = true;
            }
            if (!isStreamNow && isStream) {
                TrayIcon.ShowBalloonTip(1000, null, "Стрим закончился", ToolTipIcon.Warning);
            }
            isStream = isStreamNow;
            streamTestTimer.Start();
        }

        void StartStreamTestTimer() {
            streamTestTimer = new Timer();
            streamTestTimer.Tick += (sender, e) => TestStream();
            streamTestTimer.Interval = STREAM_TIMER_RESOLUTION;
            streamTestTimer.Enabled = Properties.Settings.Default.watchVidimo;
            // Check once immediately after run
            if (Properties.Settings.Default.watchVidimo)
                TestStream();
        }

        void TestRadio() {
            radioTestTimer.Stop();

            try {
                byte[] pageBytes = new WebClient().DownloadData(URLs.radioState);
                string pageText = Encoding.UTF8.GetString(pageBytes);

                string[] delimited = pageText.Split('\n');
                Dictionary<string, string> d = new Dictionary<string, string>();
                for (int i=0; delimited.Length-i >= 2; i+=2)
                    d.Add(delimited[i], delimited[i + 1]);

                if (d.ContainsKey("cover")) {
                    int newCover = int.Parse(d["cover"]);
                    if (newCover != lastCover) {
                        lastCover = newCover;
                        CoverBox.LoadAsync(URLs.RadioCover(newCover));
                    }
                }

                StringBuilder trackSb = new StringBuilder();
                if (d.ContainsKey("Artist"))
                    trackSb.Append(d["Artist"]);
                if (d.ContainsKey("Title"))
                    trackSb.Append($" — {d["Title"]}");
                /*
                if (d.ContainsKey("Album"))
                    trackSb.Append($" из альбома {d["Album"]}");
                    */
                string track = trackSb.ToString();

#if DEBUG
                Debugger.Log(5, "Получили имя трека", track + "\n");
#endif

                coverTt.SetToolTip(CoverBox, track);

                if (Properties.Settings.Default.warnAboutLive && d.ContainsKey("isLive")) {
                    bool isLive = int.Parse(d["isLive"]) == 1;
                    if (isLive && !wasLive)
                        MessageBox.Show("Началось живое вещщание\nПодключайся");
                    wasLive = isLive;
                }

                if (track.Length > 0)
                    UpdateCoverMenu(track);

            } catch (WebException e) when (e.Status is WebExceptionStatus.ProtocolError && e.Response is HttpWebResponse r) {
                // If we have 404, it means stream hasn't been started
                // All is OK. Just skip it
                if (r.StatusCode != HttpStatusCode.NotFound)
                    HandleException(e);
            } catch (Exception e) {
                HandleException(e);
            }

            radioTestTimer.Start();
        }

        private void OpenGoogleTrackSearch(object s, EventArgs e) => Process.Start(URLs.Googel((s as MenuItem).Text.Substring(4)));

        void UpdateCoverMenu(string track) {
            bool menuExists = CoverBox.ContextMenu != null;

            if (menuExists && CoverBox.ContextMenu.MenuItems[0].Text == track)
                return;

            if (Properties.Settings.Default.warnAboutTrackChange)
                TrayIcon.ShowBalloonTip(0, null, track, ToolTipIcon.Info);

            int newMenuLength = 1;
            if (menuExists) newMenuLength = (CoverBox.ContextMenu.MenuItems.Count < 10)
                    ? CoverBox.ContextMenu.MenuItems.Count + 1
                    : 10;

            MenuItem[] mi = new MenuItem[newMenuLength];
            mi[0] = new MenuItem($" 0. {track}", OpenGoogleTrackSearch);
            for (int i = 1; i < newMenuLength; i++) {
                mi[i] = CoverBox.ContextMenu.MenuItems[i - 1];
                mi[i].Text = $"-{i}. {mi[i].Text.Substring(4)}";
            }

            CoverBox.ContextMenu = new ContextMenu(mi);
        }

        void StartRadioTestTimer() {
            radioTestTimer = new Timer();
            radioTestTimer.Tick += (sender, e) => TestRadio();
            radioTestTimer.Interval = RADIO_TIMER_RESOLUTION;
            radioTestTimer.Enabled = Properties.Settings.Default.watchRadio;
            // Check once immediately after run
            if (Properties.Settings.Default.watchRadio)
                TestRadio();
        }

        

        // Captcha answer
        private void CaptchaAnswer_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.R) {
                ReloadCaptcha();
                e.Handled = true;
            }
        }
        void CaptchaAnswer_KeyPress(object sender, KeyPressEventArgs e) {
            if ((Keys)e.KeyChar == Keys.Enter) {
                Kukarek();
                e.Handled = true;
            }
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;

            if (!char.IsControl(e.KeyChar) && CaptchaAnswer.Text.Length >= 5) {
                CaptchaAnswer.Text = CaptchaAnswer.Text.Substring(0, 5);
                CaptchaAnswer.Select(5, 0);
                e.Handled = true;
            }
        }
        void CaptchaAnswer_Enter(object sender, EventArgs e) {
            CaptchaAnswer.SelectAll();
        }


        // Main form
        void MainForm_Load(object sender, EventArgs e) {
            AppDomain.CurrentDomain.ProcessExit += (azaza, ololo) => TrayIcon.Dispose();
            AppDomain.CurrentDomain.ProcessExit += (azaza, ololo) => Properties.Settings.Default.Save();

            // Visuals
            ClientSize = new Size(
                KukarekBox.Width,
                KukarekBox.Height + 1 + CaptchaPicture.Height + 1 + CaptchaAnswer.Height
                );
            MinimumSize = new Size(
                CaptchaPicture.Width,
                22 + CaptchaPicture.Height + CaptchaAnswer.Height
                );

            TrayIcon.Icon = Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().Location);

            TopMost = Properties.Settings.Default.topMost;

            SetDocking();
            GlobHotkey();
            MakeTray();
            Recolor(Properties.Settings.Default.backColor, Properties.Settings.Default.foreColor);

            ContextMenu = LeftSymbolsLabel.ContextMenu = IdLabel.ContextMenu = StatusLabel.ContextMenu = CaptchaPicture.ContextMenu = TrayIcon.ContextMenu;

            coverTt.SetToolTip(CoverBox, "");

            // Window
            BringToFront();
            Activate();
            KukarekBox.Focus();

            // Internets
            if (Properties.Settings.Default.useGodGrace)
                GodGraceVisualUpdate();
            else
                ReloadCaptcha();

            StartStreamTestTimer();
            StartRadioTestTimer();

#if DEBUG
            IdLabel.Text = "DEBUGDEBUGDEBUG";
            StatusLabel.Text = "DEB";
            Debugger.Log(5, "ALARM", "ВЫРУБИ ОТЛАДКУ ПРИ РЕЛИЗЕ\n");
#endif

        }
        void MainForm_Activated(object sender, EventArgs e) {
            KukarekBox.Focus();
        }


        // Captcha image
        private void CaptchaPicture_MouseClick(object sender, MouseEventArgs e) {
            if (!Properties.Settings.Default.useGodGrace && e.Button == MouseButtons.Left)
                ReloadCaptcha();
        }

        
        // Kukarek text
        void KukarekBox_KeyPress(object sender, KeyPressEventArgs e) {
            int left = 500 - KukarekBox.Text.Length;
            if (left <= 0)
                e.Handled = true;
        }
        void KukarekBox_TextChanged(object sender, EventArgs e) {
            int left = 500 - KukarekBox.Text.Length;
            LeftSymbolsLabel.Text = left.ToString();
        }


        // Tray icon
        private void TrayIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                NativeMethods.SendMessage(Handle, GlobalHotkey.Constants.WM_HOTKEY_MSG_ID, 0, 0);

        }

    }
}
