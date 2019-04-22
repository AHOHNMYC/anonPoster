// USE_CHMO — символ условной компиляции, позволяет чмориться :з

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;


namespace anonPoster {
    public partial class MainForm : Form {
        // Global variables
        int CaptchaID;  // updates by ReloadCaptcha()
        Image captchaOrig;
        Random mainRandom = new Random();
#if USE_CHMO
        Chmo mainChmo = new Chmo();
#endif
        WebClient captchaWC = new WebClient();
        WebClient postWC = new WebClient();
        bool hunspellLoaded = false;

        public LastTracksMenu ltm;
        public RadioWatcher rw;
        public VideoWatcher vw;
        public ScheduleWatcher sw;
        public Streams streamsForm;
        public ToolTip coverTt = new ToolTip { IsBalloon = true };

        
        enum spellCheckers {
            chmo,
            hunspell
        }

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
            HunspellCheckText();
        }


        public MainForm() {
            if (Program.startInBackground || Properties.Settings.Default.startInBackground)
                System.Threading.SynchronizationContext.Current.Post((obj) => {
                    Visible = false;
                }, null);


            InitializeComponent();

            PlayPadioButton.Visible = File.Exists("MediaFoundation.dll");
        }

        void GlobHotkey() {
            new GlobalHotkey(GlobalHotkey.Constants.CTRL + GlobalHotkey.Constants.SHIFT, Keys.K, this).Register();
        }

        void ToggleMenuRadiobutton(object s) {
            foreach (MenuItem m in ((MenuItem)s).Parent.MenuItems)
                if (m.RadioCheck)
                    m.Checked = false;
            ((MenuItem)s).Checked = true;
        }

        public void ShowStreamsForm() {
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

        private void OnMenuChangeAudioQuality(object s, byte quality) {
            if (Properties.Settings.Default.audioQuality == quality)
                return;

            Properties.Settings.Default.audioQuality = quality;
            Properties.Settings.Default.Save();

            ToggleMenuRadiobutton(s);

            if (audio != null)
                startAudioPlayer();
        }

        void MakeContextMenu() {
            ContextMenu = LeftSymbolsLabel.ContextMenu = IdLabel.ContextMenu = StatusLabel.ContextMenu = CaptchaPicture.ContextMenu = TrayIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Радио Анонимус =>", (s, e) => Process.Start(URLs.radio)),
                new MenuItem("Видимо", (s, e) => ShowStreamsForm()),
                new MenuItem("-"),
                new MenuItem("Подмять говно", (s, e) => {
                    Properties.Settings.Default.topMost = ((MenuItem)s).Checked = TopMost = !TopMost;
                    Properties.Settings.Default.Save();
                }) { Checked = Properties.Settings.Default.topMost },
                new MenuItem("Боевой окрас", new MenuItem[] {
                    new MenuItem("Белый", (s, e) => OnMenuChangeColor(s, Color.White, Color.Black)) { RadioCheck = true },
                    new MenuItem("Чёрный", (s, e) => OnMenuChangeColor(s, Color.Black, Color.White)) { RadioCheck = true },
                    new MenuItem("Стандартный", (s, e) => OnMenuChangeColor(s, DefaultBackColor, DefaultForeColor)) { RadioCheck = true },
                    new MenuItem("Нестандартный", (s, e) => {
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
                new MenuItem("Скрывать ошибки", (s, e) => {
                    ((MenuItem)s).Checked = Properties.Settings.Default.hideErrors = !Properties.Settings.Default.hideErrors;
                    Properties.Settings.Default.Save();
                }) { Checked = Properties.Settings.Default.hideErrors },
                new MenuItem("Проверка текста", new MenuItem[] {
#if USE_CHMO
                    new MenuItem("Чмо", (s, e) => {
                        ((MenuItem)s).Checked = Properties.Settings.Default.useChmo = !Properties.Settings.Default.useChmo;
                        Properties.Settings.Default.Save();
                    }) { Checked = Properties.Settings.Default.useChmo },
#endif
                    new MenuItem("Hunspell", (s, e) => {
                        if (((MenuItem)s).Checked = Properties.Settings.Default.useHunspell = !Properties.Settings.Default.useHunspell)
                            LoadHunspell();
                        Properties.Settings.Default.Save();
                    }) { Checked = Properties.Settings.Default.useHunspell },
                }),

                new MenuItem("Запускаться в фоне", (s, e) => {
                    ((MenuItem)s).Checked = Properties.Settings.Default.startInBackground = !Properties.Settings.Default.startInBackground;
                    Properties.Settings.Default.Save();
                }) { Checked = Properties.Settings.Default.startInBackground },
                new MenuItem("Автозагрузка с системой", (s, e) => {
                    MenuItem mi = (MenuItem)s;
                    mi.Checked = Autostart.Set(!mi.Checked);
                }) { Checked = Autostart.Check() },
                new MenuItem("Следить за видимом", (s, e) => {
                    Properties.Settings.Default.watchVidimo = ((MenuItem)s).Checked = vw.ToggleStreamTestTimer();
                    Properties.Settings.Default.Save();
                }) { Checked = Properties.Settings.Default.watchVidimo },
                new MenuItem("Следить за радивой", new MenuItem[] {
                    new MenuItem("Следить за радивой", (s, e) => {
                        Properties.Settings.Default.watchRadio = ((MenuItem)s).Checked = rw.ToggleRadioTestTimer();
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
#if DEBUG
                new MenuItem("Следить за расписанием", (s, e) => {
                    Properties.Settings.Default.watchSched = ((MenuItem)s).Checked = sw.ToggleSchedTestTimer();
                    Properties.Settings.Default.Save();
                }) { Checked = Properties.Settings.Default.watchSched },
#endif
                new MenuItem("Обход капчи (эксперим.)", (s, e) => {
                    ((MenuItem)s).Checked = Properties.Settings.Default.useGodGrace = !Properties.Settings.Default.useGodGrace;
                    Properties.Settings.Default.Save();
                    GodGraceVisualUpdate();
                }) { Checked = Properties.Settings.Default.useGodGrace },
                new MenuItem("-"),
                new MenuItem("Что умеет?", (s, e) => HelpMeee()),
                new MenuItem("Выход", (s, e) => Close())
            });


            // Контекстное меню на кнопке воспроизведения радивы
            PlayPadioButton.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("ХТТПС", (s, e) => {
                    ((MenuItem)s).Checked = Properties.Settings.Default.audioUseSSL = !Properties.Settings.Default.audioUseSSL;
                    Properties.Settings.Default.Save();

                    if (audio != null)
                        startAudioPlayer();
                }) { Checked = Properties.Settings.Default.audioUseSSL },
                new MenuItem("192 кбпс", (s, e) => OnMenuChangeAudioQuality(s, 192)) { RadioCheck = true,
                    Checked = Properties.Settings.Default.audioQuality == 192},
                new MenuItem("64 кбпс", (s, e) => OnMenuChangeAudioQuality(s, 64)) { RadioCheck = true,
                    Checked = Properties.Settings.Default.audioQuality == 64},
                new MenuItem("12 кбпс", (s, e) => OnMenuChangeAudioQuality(s, 12)) { RadioCheck = true,
                    Checked = Properties.Settings.Default.audioQuality == 12},
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
                    byte[] pageBytes = captchaWC.DownloadData(URLs.radioFeedback);
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
                        MessageBox.Show(this, "Охуеть. Капча пропала", "Ёбаный пиздец", MessageBoxButtons.OK, MessageBoxIcon.Question);
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

        public void HandleException(Exception e) {
#if DEBUG
            Debugger.Break();
#endif
            if (e is WebException we && we.Status == WebExceptionStatus.ConnectFailure)
                ChangeCaptchaImage(Properties.Resources.captchaNoInternet);
            else
                if (!Properties.Settings.Default.hideErrors)
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
                    ShowStreamsForm();
                }
            }
            base.WndProc(ref m);
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData) {
            switch (keyData) {
                case Keys.Escape:
                    Visible = false;
                    return true;
                case Keys.F1:
                    HelpMeee();
                    return true;
                case Keys.Control | Keys.Enter:
                    Kukarek();
                    return true;
                case Keys.Apps: // https://en.wikipedia.org/wiki/Menu_key
                case Keys.F10:
                    ContextMenu.Show(this, new Point());
                    return true;
            }
            return base.ProcessDialogKey(keyData);
        }



        bool Kukarek() {
            if (KukarekBox.TextLength == 0) {
                TrayIcon.ShowBalloonTip(1000, null, "Введи что-нибудь", ToolTipIcon.Warning);
                KukarekBox.Focus();
                return false;
            }


#if USE_CHMO
            if (Properties.Settings.Default.useChmo) {
                string[] chmoResult = mainChmo.CheckString(KukarekBox.Text);
                if (chmoResult.Length > 0) {
                    string question = $"Мы обнаружили у вас странные слова:\n\n{string.Join(", ", chmoResult)}\n\nЖелаете вернуться к редактированию сообщения и исправить их?";
                    if (DialogResult.Yes == MessageBox.Show(this, question, null, MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                        KukarekBox.Focus();
                        return false;
                    }
                }
            }
#endif

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
                    .Append("&left=").Append(500 - KukarekBox.TextLength)
                    .Append("&msg=").Append(Uri.EscapeDataString(KukarekBox.Text))
                    .Append("&check=").Append(CaptchaAnswer.Text);
            }

#if DEBUG
            Debugger.Log(5, "ALARM", postDataSb.ToString());
#endif

            Enabled = false;
            try {
                postWC.Headers.Clear();
                if (Properties.Settings.Default.useGodGrace)
                    postWC.Headers.Add(HttpRequestHeader.UserAgent, "AnonFM Player for Android");
                postWC.Headers.Add(HttpRequestHeader.Referer, URLs.radioFeedback);
                string response = Encoding.UTF8.GetString(
                    postWC.UploadData(URLs.radioFeedback, "POST", Encoding.ASCII.GetBytes(postDataSb.ToString()))
                    );

                if (response.IndexOf("Отправлено") > -1) {
                    // Extract userID
                    int idStart = response.IndexOf("strong") + 7;
                    int idLength = response.LastIndexOf("</strong") - idStart;
                    IdLabel.Text = response.Substring(idStart, idLength);

                    StatusLabel.Text = "OK";

                    if (!Properties.Settings.Default.useGodGrace)
                        ReloadCaptcha();

                    KukarekBox.Clear();
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


        // Hunspell
        bool LoadHunspell() {
            if (!hunspellLoaded) {

                string dllPath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}{Path.DirectorySeparatorChar}NHunspell.dll";
                if (!SpellHunspell.LoadDll(dllPath))
                    return false;

                if (!SpellHunspell.Load(Assembly.GetEntryAssembly().Location))
                    return false;

                hunspellLoaded = true;
                HunspellCheckText();
            }

            return true;
        }

        void HunspellCheckText() {
            if (Properties.Settings.Default.useHunspell && hunspellLoaded)
                SpellHunspell.ValidateTextInRich(KukarekBox);
        }




        void HelpMeee() {
            MessageBox.Show(this, @"Умеет радиачевать капчу и срать в кукарекалку

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

♣ Через контекстное меню на странной кнопке можно выбрать качество внучания

• Есть ключ /bg для запуска в фоне

└ Софтина может предупреждать о запуске видимопотока", "Что умеет?", MessageBoxButtons.OK, MessageBoxIcon.Information);

            const string dev = "c0d3d by AHOHNMYC, 2o18\n\nПроверить обновления?";
            if (DialogResult.Yes == MessageBox.Show(this, dev, "d3ve1oler", MessageBoxButtons.YesNo, MessageBoxIcon.Information)) {
                Process.Start(URLs.selfUpdate);
            }
        }


        private void SetAnchor(AnchorStyles ans, Control[] controls) {
            foreach (Control control in controls)
                control.Anchor = ans;
        }

        void SetDocking() {
            ResizePicture.Top = Height - ResizePicture.Height;
            ResizePicture.Left = Width - ResizePicture.Width;

            AnchorStyles b = AnchorStyles.Bottom;
            AnchorStyles br = AnchorStyles.Bottom | AnchorStyles.Right;
            AnchorStyles bl = AnchorStyles.Bottom | AnchorStyles.Left;
            AnchorStyles around = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;

            SetAnchor(b, new Control[] { CaptchaPicture, CaptchaAnswer, CoverBox });
            SetAnchor(br, new Control[] { IdLabel, ResizePicture });
            SetAnchor(bl, new Control[] { StatusLabel, PlayPadioButton, LeftSymbolsLabel });
            SetAnchor(around, new Control[] { KukarekBox });
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
            AppDomain.CurrentDomain.ProcessExit += (azaza, ololo) => destructAudioPlayer();

            if (Properties.Settings.Default.useHunspell)
                LoadHunspell();

            // Visuals
            ClientSize = new Size(
                KukarekBox.Width,
                KukarekBox.Height + 1 + CaptchaPicture.Height + 1 + CaptchaAnswer.Height
                );
            MinimumSize = new Size(
                CaptchaPicture.Width,
                22 + CaptchaPicture.Height + CaptchaAnswer.Height
                );

            TrayIcon.Icon = Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);

            SetDocking();
            GlobHotkey();
            MakeContextMenu();
            Recolor(Properties.Settings.Default.backColor, Properties.Settings.Default.foreColor);

            coverTt.SetToolTip(CoverBox, "");

            string[] verbs = new string[] { "Зву", "Кри", "Мол", "Вну", "Ы", "Сер", "Ди", "Отве", "Важни", "Вклю", "", "", "" };
            int r = mainRandom.Next(verbs.Length);
            PlayPadioButton.Text = $"{verbs[r]}чать";

            // Чтобы после вставки CJK не сбрасывался ClearType
            KukarekBox.LanguageOption = RichTextBoxLanguageOptions.DualFont;

            // Window
            BringToFront();
            Activate();
            KukarekBox.Focus();

            // Internets
            if (Properties.Settings.Default.useGodGrace)
                GodGraceVisualUpdate();
            else
                ReloadCaptcha();

            // Watchers
            ltm = new LastTracksMenu(this);
            rw = new RadioWatcher(this);
            vw = new VideoWatcher(this);
            sw = new ScheduleWatcher(this);

#if DEBUG
            //KukarekBox.Text = @"Два бомжа, Валера и Петюх, сидели в углу руинированной квартиры на куче влажного тряпья. В выбитом окне сиял тонкий месяц. Бомжи были пьяны. И допивали бутылку «Русской». Они начали пить с раннего утра на Ярославском вокзале: четвертинка «Истока», полбатона белого хлеба, куриные объедки из гриль-бара. Потом доехали до Сокольников, где в парке насобирали пустых бутылок, сдали и продолжили: три бутылки пива «Очаковское», две булочки с маком. После они выспались на лавочке, доехали до Новодевичьего монастыря, где до вечера просили милостыню. Ее хватило на бутылку «Русской».";
            KukarekBox.Text = "Мама мыла азаза";
            //KukarekBox.Text = "Азаза мыл раму";

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
        void KukarekBox_TextChanged(object sender, EventArgs e) {
            int left = 500 - KukarekBox.TextLength;
            LeftSymbolsLabel.Text = left.ToString();

            HunspellCheckText();
        }


        // Tray icon
        private void TrayIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                NativeMethods.SendMessage(Handle, GlobalHotkey.Constants.WM_HOTKEY_MSG_ID, 0, 0);
        }

        // Play radio
        CPlayer audio;
        private void destructAudioPlayer() {
            if (audio != null) {
                audio.Shutdown();
                audio = null;
            }
        }
        private void startAudioPlayer() {
            destructAudioPlayer();

            bool ssl = Properties.Settings.Default.audioUseSSL;

            string stream;
            switch (Properties.Settings.Default.audioQuality) {
                case 192: stream = URLs.audio192; break;
                case 64:  stream = URLs.audio64;  break;
                case 12:  stream = URLs.audio12;  break;
                default:  stream = URLs.audio64;  break;
            }

            audio = new CPlayer(PlayPadioButton.Handle, Handle);
            audio.OpenURL($"{(ssl ? URLs.audioSSL : URLs.audio)}{stream}");
        }
        private void PlayRadioButton_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left)
                return;

            if (PlayPadioButton.Checked)
                startAudioPlayer();
            else
                destructAudioPlayer();
        }

    }
}
