namespace anonPoster {
    partial class Schedule {
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
            this.scheduleText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // scheduleText
            // 
            this.scheduleText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scheduleText.Location = new System.Drawing.Point(0, 0);
            this.scheduleText.Name = "scheduleText";
            this.scheduleText.ReadOnly = true;
            this.scheduleText.Size = new System.Drawing.Size(487, 310);
            this.scheduleText.TabIndex = 0;
            // 
            // Schedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 310);
            this.Controls.Add(this.scheduleText);
            this.Name = "Schedule";
            this.Text = "Schedule";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox scheduleText;
    }
}