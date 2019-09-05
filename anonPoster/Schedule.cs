using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace anonPoster {
    public partial class Schedule : Form {
        public Schedule() {
            InitializeComponent();
        }

        public void ClearSchedule() {
            scheduleText.Clear();
        }
        public void AddEvent(string eventDescription) {
            scheduleText.AppendText(eventDescription + '\n');
        }
    }
}
