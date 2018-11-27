namespace anonPoster {
    partial class MainForm {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.KukarekBox = new System.Windows.Forms.RichTextBox();
            this.CaptchaAnswer = new System.Windows.Forms.TextBox();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.LeftSymbolsLabel = new System.Windows.Forms.Label();
            this.IdLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.CaptchaPicture = new System.Windows.Forms.PictureBox();
            this.ResizePicture = new System.Windows.Forms.PictureBox();
            this.CoverBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CaptchaPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CoverBox)).BeginInit();
            this.SuspendLayout();
            // 
            // KukarekBox
            // 
            this.KukarekBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.KukarekBox.Location = new System.Drawing.Point(0, 0);
            this.KukarekBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.KukarekBox.Name = "KukarekBox";
            this.KukarekBox.Size = new System.Drawing.Size(511, 110);
            this.KukarekBox.TabIndex = 0;
            this.KukarekBox.Text = "";
            this.KukarekBox.TextChanged += new System.EventHandler(this.KukarekBox_TextChanged);
            this.KukarekBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KukarekBox_KeyPress);
            // 
            // CaptchaAnswer
            // 
            this.CaptchaAnswer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CaptchaAnswer.Location = new System.Drawing.Point(217, 180);
            this.CaptchaAnswer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CaptchaAnswer.Name = "CaptchaAnswer";
            this.CaptchaAnswer.Size = new System.Drawing.Size(77, 23);
            this.CaptchaAnswer.TabIndex = 2;
            this.CaptchaAnswer.Text = "meow :з";
            this.CaptchaAnswer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CaptchaAnswer.Enter += new System.EventHandler(this.CaptchaAnswer_Enter);
            this.CaptchaAnswer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CaptchaAnswer_KeyDown);
            this.CaptchaAnswer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CaptchaAnswer_KeyPress);
            // 
            // TrayIcon
            // 
            this.TrayIcon.Text = "Радио\r\nАнонимус";
            this.TrayIcon.Visible = true;
            this.TrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseClick);
            // 
            // LeftSymbolsLabel
            // 
            this.LeftSymbolsLabel.Location = new System.Drawing.Point(12, 112);
            this.LeftSymbolsLabel.Name = "LeftSymbolsLabel";
            this.LeftSymbolsLabel.Size = new System.Drawing.Size(32, 17);
            this.LeftSymbolsLabel.TabIndex = 4;
            this.LeftSymbolsLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragByMouse);
            // 
            // IdLabel
            // 
            this.IdLabel.Location = new System.Drawing.Point(361, 112);
            this.IdLabel.Name = "IdLabel";
            this.IdLabel.Size = new System.Drawing.Size(139, 17);
            this.IdLabel.TabIndex = 5;
            this.IdLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.IdLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragByMouse);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Location = new System.Drawing.Point(12, 182);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(32, 17);
            this.StatusLabel.TabIndex = 7;
            this.StatusLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragByMouse);
            // 
            // CaptchaPicture
            // 
            this.CaptchaPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CaptchaPicture.InitialImage = null;
            this.CaptchaPicture.Location = new System.Drawing.Point(155, 110);
            this.CaptchaPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CaptchaPicture.Name = "CaptchaPicture";
            this.CaptchaPicture.Size = new System.Drawing.Size(200, 70);
            this.CaptchaPicture.TabIndex = 1;
            this.CaptchaPicture.TabStop = false;
            this.CaptchaPicture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CaptchaPicture_MouseClick);
            // 
            // ResizePicture
            // 
            this.ResizePicture.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.ResizePicture.Image = global::anonPoster.Properties.Resources.arrow;
            this.ResizePicture.Location = new System.Drawing.Point(487, 176);
            this.ResizePicture.Name = "ResizePicture";
            this.ResizePicture.Size = new System.Drawing.Size(24, 23);
            this.ResizePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ResizePicture.TabIndex = 6;
            this.ResizePicture.TabStop = false;
            this.ResizePicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResizeByMouse);
            // 
            // CoverBox
            // 
            this.CoverBox.ErrorImage = null;
            this.CoverBox.InitialImage = null;
            this.CoverBox.Location = new System.Drawing.Point(361, 143);
            this.CoverBox.Name = "CoverBox";
            this.CoverBox.Size = new System.Drawing.Size(60, 60);
            this.CoverBox.TabIndex = 8;
            this.CoverBox.TabStop = false;
            this.CoverBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragByMouse);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 207);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.LeftSymbolsLabel);
            this.Controls.Add(this.CaptchaPicture);
            this.Controls.Add(this.IdLabel);
            this.Controls.Add(this.ResizePicture);
            this.Controls.Add(this.KukarekBox);
            this.Controls.Add(this.CaptchaAnswer);
            this.Controls.Add(this.CoverBox);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 93);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Кукареку 0.0.0α";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragByMouse);
            ((System.ComponentModel.ISupportInitialize)(this.CaptchaPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CoverBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox KukarekBox;
        private System.Windows.Forms.PictureBox CaptchaPicture;
        private System.Windows.Forms.TextBox CaptchaAnswer;
        private System.Windows.Forms.Label LeftSymbolsLabel;
        private System.Windows.Forms.Label IdLabel;
        private System.Windows.Forms.PictureBox ResizePicture;
        private System.Windows.Forms.Label StatusLabel;
        public System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.PictureBox CoverBox;
    }
}

