namespace anonPoster {
    partial class Streams {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.link1 = new System.Windows.Forms.TextBox();
            this.link2 = new System.Windows.Forms.TextBox();
            this.link3 = new System.Windows.Forms.TextBox();
            this.youtubeLink = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mpv2 = new System.Windows.Forms.Button();
            this.rec2 = new System.Windows.Forms.Button();
            this.mpv1 = new System.Windows.Forms.Button();
            this.mpc2 = new System.Windows.Forms.Button();
            this.mpc1 = new System.Windows.Forms.Button();
            this.rec1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rec3 = new System.Windows.Forms.Button();
            this.mpc3 = new System.Windows.Forms.Button();
            this.mpv3 = new System.Windows.Forms.Button();
            this.CheckCmdLine = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // link1
            // 
            this.link1.Location = new System.Drawing.Point(6, 19);
            this.link1.Name = "link1";
            this.link1.Size = new System.Drawing.Size(278, 20);
            this.link1.TabIndex = 0;
            // 
            // link2
            // 
            this.link2.Location = new System.Drawing.Point(6, 45);
            this.link2.Name = "link2";
            this.link2.Size = new System.Drawing.Size(278, 20);
            this.link2.TabIndex = 4;
            // 
            // link3
            // 
            this.link3.Location = new System.Drawing.Point(6, 19);
            this.link3.Name = "link3";
            this.link3.Size = new System.Drawing.Size(278, 20);
            this.link3.TabIndex = 8;
            // 
            // youtubeLink
            // 
            this.youtubeLink.AutoSize = true;
            this.youtubeLink.Location = new System.Drawing.Point(507, 148);
            this.youtubeLink.Name = "youtubeLink";
            this.youtubeLink.Size = new System.Drawing.Size(32, 13);
            this.youtubeLink.TabIndex = 13;
            this.youtubeLink.TabStop = true;
            this.youtubeLink.Text = "Ютуб";
            this.youtubeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.youtubeLink_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.link2);
            this.groupBox1.Controls.Add(this.link1);
            this.groupBox1.Controls.Add(this.mpv2);
            this.groupBox1.Controls.Add(this.rec2);
            this.groupBox1.Controls.Add(this.mpv1);
            this.groupBox1.Controls.Add(this.mpc2);
            this.groupBox1.Controls.Add(this.mpc1);
            this.groupBox1.Controls.Add(this.rec1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(538, 73);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Основной поток";
            // 
            // mpv2
            // 
            this.mpv2.Location = new System.Drawing.Point(290, 43);
            this.mpv2.Name = "mpv2";
            this.mpv2.Size = new System.Drawing.Size(75, 23);
            this.mpv2.TabIndex = 5;
            this.mpv2.Text = "MPV";
            this.mpv2.UseVisualStyleBackColor = true;
            this.mpv2.Click += new System.EventHandler(this.mpv2_Click);
            // 
            // rec2
            // 
            this.rec2.Location = new System.Drawing.Point(452, 43);
            this.rec2.Name = "rec2";
            this.rec2.Size = new System.Drawing.Size(75, 23);
            this.rec2.TabIndex = 7;
            this.rec2.Text = "Запись";
            this.rec2.UseVisualStyleBackColor = true;
            this.rec2.Click += new System.EventHandler(this.rec2_Click);
            // 
            // mpv1
            // 
            this.mpv1.Location = new System.Drawing.Point(290, 17);
            this.mpv1.Name = "mpv1";
            this.mpv1.Size = new System.Drawing.Size(75, 23);
            this.mpv1.TabIndex = 1;
            this.mpv1.Text = "MPV";
            this.mpv1.UseVisualStyleBackColor = true;
            this.mpv1.Click += new System.EventHandler(this.mpv1_Click);
            // 
            // mpc2
            // 
            this.mpc2.Location = new System.Drawing.Point(371, 43);
            this.mpc2.Name = "mpc2";
            this.mpc2.Size = new System.Drawing.Size(75, 23);
            this.mpc2.TabIndex = 6;
            this.mpc2.Text = "MPC-HC";
            this.mpc2.UseVisualStyleBackColor = true;
            this.mpc2.Click += new System.EventHandler(this.mpc2_Click);
            // 
            // mpc1
            // 
            this.mpc1.Location = new System.Drawing.Point(371, 17);
            this.mpc1.Name = "mpc1";
            this.mpc1.Size = new System.Drawing.Size(75, 23);
            this.mpc1.TabIndex = 2;
            this.mpc1.Text = "MPC-HC";
            this.mpc1.UseVisualStyleBackColor = true;
            this.mpc1.Click += new System.EventHandler(this.mpc1_Click);
            // 
            // rec1
            // 
            this.rec1.Location = new System.Drawing.Point(452, 17);
            this.rec1.Name = "rec1";
            this.rec1.Size = new System.Drawing.Size(75, 23);
            this.rec1.TabIndex = 3;
            this.rec1.Text = "Запись";
            this.rec1.UseVisualStyleBackColor = true;
            this.rec1.Click += new System.EventHandler(this.rec1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rec3);
            this.groupBox2.Controls.Add(this.link3);
            this.groupBox2.Controls.Add(this.mpc3);
            this.groupBox2.Controls.Add(this.mpv3);
            this.groupBox2.Location = new System.Drawing.Point(12, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(538, 47);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Бомжепоток";
            // 
            // rec3
            // 
            this.rec3.Location = new System.Drawing.Point(452, 17);
            this.rec3.Name = "rec3";
            this.rec3.Size = new System.Drawing.Size(75, 23);
            this.rec3.TabIndex = 11;
            this.rec3.Text = "Запись";
            this.rec3.UseVisualStyleBackColor = true;
            this.rec3.Click += new System.EventHandler(this.rec3_Click);
            // 
            // mpc3
            // 
            this.mpc3.Location = new System.Drawing.Point(371, 17);
            this.mpc3.Name = "mpc3";
            this.mpc3.Size = new System.Drawing.Size(75, 23);
            this.mpc3.TabIndex = 10;
            this.mpc3.Text = "MPC-HC";
            this.mpc3.UseVisualStyleBackColor = true;
            this.mpc3.Click += new System.EventHandler(this.mpc3_Click);
            // 
            // mpv3
            // 
            this.mpv3.Location = new System.Drawing.Point(290, 17);
            this.mpv3.Name = "mpv3";
            this.mpv3.Size = new System.Drawing.Size(75, 23);
            this.mpv3.TabIndex = 9;
            this.mpv3.Text = "MPV";
            this.mpv3.UseVisualStyleBackColor = true;
            this.mpv3.Click += new System.EventHandler(this.mpv3_Click);
            // 
            // CheckCmdLine
            // 
            this.CheckCmdLine.AutoSize = true;
            this.CheckCmdLine.Location = new System.Drawing.Point(12, 144);
            this.CheckCmdLine.Name = "CheckCmdLine";
            this.CheckCmdLine.Size = new System.Drawing.Size(160, 17);
            this.CheckCmdLine.TabIndex = 12;
            this.CheckCmdLine.Text = "Редактировать комстроку";
            this.CheckCmdLine.UseVisualStyleBackColor = true;
            this.CheckCmdLine.CheckedChanged += new System.EventHandler(this.CheckCmdLine_CheckedChanged);
            // 
            // Streams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 172);
            this.Controls.Add(this.CheckCmdLine);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.youtubeLink);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Streams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Видимопотоки";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox link1;
        private System.Windows.Forms.TextBox link2;
        private System.Windows.Forms.TextBox link3;
        private System.Windows.Forms.LinkLabel youtubeLink;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button mpv2;
        private System.Windows.Forms.Button rec2;
        private System.Windows.Forms.Button mpv1;
        private System.Windows.Forms.Button mpc2;
        private System.Windows.Forms.Button mpc1;
        private System.Windows.Forms.Button rec1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button rec3;
        private System.Windows.Forms.Button mpc3;
        private System.Windows.Forms.Button mpv3;
        private System.Windows.Forms.CheckBox CheckCmdLine;
    }
}