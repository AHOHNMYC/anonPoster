using System.IO;
using System.Media;
using System.Windows.Forms;

namespace anonPoster {
    public partial class NY : Form {
        public NY() {
            InitializeComponent();
        }

        private MemoryStream genYolotchka() {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.Write("WAVE");
//            sw.Write();


            ms.Position = 0;
            return ms;
        }

        private void NY_MouseClick(object sender, MouseEventArgs e) {
            MessageBox.Show("Новый Год — это странная вещь. Но по сложившейся традиции в этот и предшествующий ему моменты надо ВЕСЕЛИТЬСЯ!!!1!\n\nИди веселиться, Анончик. А Радио Анонимус тебе в этом поможет!");
            return;
#pragma warning disable CS0162
        SoundPlayer s = new SoundPlayer(genYolotchka());
            s.Load();
            s.Play();
#pragma warning restore CS0162
        }
    }
}
