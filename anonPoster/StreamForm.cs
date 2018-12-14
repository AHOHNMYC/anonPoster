using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows.Forms;
using static System.Environment;

namespace anonPoster {
    public partial class Streams : Form {
        MainForm mainForm;
        string MPV;
        string MPC;
        string ffmpeg;

        public Streams(MainForm main) {
            InitializeComponent();
            mainForm = main;
            CheckSoft();
            CheckCmdLine.Checked = Properties.Settings.Default.checkCmdLine;
            link1.Text = URLs.mainStream;
            link2.Text = URLs.mainStreamRtmp;
            link3.Text = URLs.bomjStream;
        }

        private void youtubeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (DialogResult.Yes == MessageBox.Show("Попробовать запустить Streamlink?", "YouTube", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                try {
                    Process.Start("streamlink", URLs.youTube);
                } catch (System.ComponentModel.Win32Exception) {
                    mainForm.TrayIcon.ShowBalloonTip(500, null, "Streamlink не найден", ToolTipIcon.Error);
                }
            } else {
                Process.Start(URLs.youTube);
            }
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData) {
            switch (keyData) {
                case Keys.Escape:
                    Close();
                    mainForm.BringToFront();
                    mainForm.Activate();
                    return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        void CheckSoft() {
            CheckMPC();
            CheckMPV();
            CheckFfmpeg();
            //MessageBox.Show($"{MPC}\n{MPV}\n{ffmpeg}");
        }

        void CheckMPC() {
            string ppp = $"{GetFolderPath(SpecialFolder.ProgramFiles)}\\MPC-HC\\";
            string mpcPath = FindFile.PathFindOnPath(new string[] { "mpc-hc64.exe", "mpc-hc.exe" }, ppp);
            if (null == mpcPath)
                foreach (Button b in new Button[] { mpc1, mpc2, mpc3 })
                    b.Enabled = false;
            MPC = mpcPath;
        }

        void CheckMPV() {
            string mpvPath = FindFile.PathFindOnPath("mpv.exe");
            if (null == mpvPath)
                foreach (Button b in new Button[] { mpv1, mpv2, mpv3 })
                    b.Enabled = false;
            MPV = mpvPath;
        }

        void CheckFfmpeg() {
            string ffmpegPath = FindFile.PathFindOnPath("ffmpeg.exe");
            if (null == ffmpegPath)
                foreach (Button b in new Button[] { rec1, rec2, rec3 })
                    b.Enabled = false;
            ffmpeg = ffmpegPath;
        }

        void CheckEditCmd(string exe, string arguments) {
            if (CheckCmdLine.Checked) {
                Prompt p = new Prompt(TopMost, arguments, "Командная строка", this, Icon, Font);
                if (p.ShowDialog() != DialogResult.OK) {
                    p.Dispose();
                    return;
                }
                arguments = p.GetModifiedText();
                p.Dispose();
            }
            if (null != arguments) {
                Process.Start(exe, arguments);
            }
        }

        void StartMPV(string stream) {
            CheckEditCmd(MPV, $"--no-fullscreen --no-ytdl {stream}");
        }
        void StartMPC(string stream) {
            CheckEditCmd(MPC, stream);
        }
        void StartRecording(string stream) {
            CheckEditCmd(ffmpeg, $"-hide_banner -i {stream} -c copy \"{GetFolderPath(SpecialFolder.Desktop)}\\anon.fm {DateTime.Now:yyyy.MM.dd hh.mm.ss}.mkv\"");
        }

        private void mpv1_Click(object s, EventArgs e) {StartMPV(link1.Text);}
        private void mpv2_Click(object s, EventArgs e) {StartMPV(link2.Text);}
        private void mpv3_Click(object s, EventArgs e) {StartMPV(link3.Text);}
        private void mpc1_Click(object s, EventArgs e) {StartMPC(link1.Text);}
        private void mpc2_Click(object s, EventArgs e) {StartMPC(link2.Text);}
        private void mpc3_Click(object s, EventArgs e) {StartMPC(link3.Text);}
        private void rec1_Click(object s, EventArgs e) {StartRecording(link1.Text);}
        private void rec2_Click(object s, EventArgs e) {StartRecording(link2.Text);}
        private void rec3_Click(object s, EventArgs e) {StartRecording(link3.Text);}

        private void CheckCmdLine_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.checkCmdLine = CheckCmdLine.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
