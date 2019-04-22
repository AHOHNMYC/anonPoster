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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.й = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ц = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.у = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.к = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.data = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.й,
            this.ц,
            this.у,
            this.к});
            this.dataGridView1.DataSource = this.eventBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(487, 310);
            this.dataGridView1.TabIndex = 0;
            // 
            // й
            // 
            this.й.Frozen = true;
            this.й.HeaderText = "Эфир";
            this.й.Name = "й";
            this.й.ReadOnly = true;
            // 
            // ц
            // 
            this.ц.Frozen = true;
            this.ц.HeaderText = "Диджей";
            this.ц.Name = "ц";
            this.ц.ReadOnly = true;
            // 
            // у
            // 
            this.у.Frozen = true;
            this.у.HeaderText = "Осталось";
            this.у.Name = "у";
            this.у.ReadOnly = true;
            // 
            // к
            // 
            this.к.Frozen = true;
            this.к.HeaderText = "Эфир";
            this.к.Name = "к";
            this.к.ReadOnly = true;
            // 
            // eventBindingSource
            // 
            this.eventBindingSource.DataSource = typeof(anonPoster.ScheduleWatcher.Event);
            // 
            // data
            // 
            this.data.DataSetName = "NewDataSet";
            this.data.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.TableName = "_";
            // 
            // Schedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 310);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Schedule";
            this.Text = "Schedule";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn й;
        private System.Windows.Forms.DataGridViewTextBoxColumn ц;
        private System.Windows.Forms.DataGridViewTextBoxColumn у;
        private System.Windows.Forms.DataGridViewTextBoxColumn к;
        private System.Windows.Forms.BindingSource eventBindingSource;
        private System.Data.DataSet data;
        private System.Data.DataTable dataTable1;
    }
}