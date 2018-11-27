using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace anonPoster {
    public partial class Prompt : Form {
        private Form _parent;
        public Prompt(bool top, string origText, string title, Form parent, Icon icon, Font font) {
            InitializeComponent();
            TopMost = top;
            richTextBox1.Text = origText;
            Text = title;
            _parent = parent;
            Icon = icon;
            Font = font;
        }
        public string GetModifiedText() {
            return richTextBox1.Text;
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData) {
            if (keyData == Keys.Escape) {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            return base.ProcessDialogKey(keyData);
        }

    }
}
