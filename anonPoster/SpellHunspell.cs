using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace anonPoster {
    static class SpellHunspell {

        private static object hunspell;
        private static MethodInfo hunspellLoad;
        private static MethodInfo hunspellSpell;

        private static void hLoad(string aff, string dic) {
            hunspellLoad.Invoke(hunspell, new object[] { aff, dic });
        }

        private static bool hSpell(string word) {
            return (bool)hunspellSpell.Invoke(hunspell, new object[] { word });
        }


        private static bool dictsFound = false;

        public static bool LoadDll(string path) {
            if (!File.Exists(path)) {
                MessageBox.Show($"Файл\n{path}\nне найден!");
                return false;
            }

            Assembly hsDll = Assembly.LoadFile(path);
            Type hsType = hsDll.GetType("NHunspell.Hunspell");
            hunspell = Activator.CreateInstance(hsType);

            hunspellLoad = hunspell.GetType().GetMethod("Load", new Type[] { typeof(string), typeof(string) });
            hunspellSpell = hunspell.GetType().GetMethod("Spell");

            return true;
        }


        /// <summary>
        /// Load all affix and dictionary files in subdirectories
        /// </summary>
        public static bool Load(string path) {

            char s = Path.DirectorySeparatorChar;
            path = Path.GetDirectoryName(path);
            string[] affixFiles = Directory.GetFiles(path, "*.aff", SearchOption.AllDirectories);

            if (affixFiles.Length == 0) {
                string text = $"Не смогли найти словарей в поддиректориях\n{path}!\n\nОткрыть ссылку, по которой можно скоммуниздить пару словарей?";
                if (DialogResult.Yes == MessageBox.Show(text, "ЕГГОГ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) {
                    Process.Start(@"https://extensions.libreoffice.org/extensions?getCategories=Dictionary&getCompatibility=any");
                    MessageBox.Show(@"1. Ищешь нужный словарь
2. Скачиваешь
3. Распаковываешь (это обычный zip-архив)
4. Идёшь в " + path + @"
5. Создаёшь папку с любым именем (например, en)
6. Копируешь в папку файлы с расширениями aff и dic
7. Перезапускаешь кукарекалку, всё должно заработать", "Что дальше делать?");
                }
                return false;
            }


            foreach (string affixFile in affixFiles) {
                // Replaces 'aff' extension with 'dic'
                string dictFile = $"{affixFile.Substring(0, affixFile.LastIndexOf('.'))}.dic";
                if (!File.Exists(dictFile)) {
                    MessageBox.Show($"Не смогли найти\n{dictFile}\n\n{affixFile}\nне будет загружен!", "ЕГГОГ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                dictsFound = true;
                hLoad(affixFile, dictFile);
            }

            return dictsFound;
        }



        /// <summary>
        /// Checks char for Russian and English symbol
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool PartOfValidableWord(char input) {
            return (input >= 0x0041) && (input <= 0x005a)
                || (input >= 0x0061) && (input <= 0x007a)
                || (input >= 0x0410) && (input <= 0x042F) // А-Я
                || (input >= 0x0430) && (input <= 0x044F) // а-я
                || (input == 0x401) || (input == 0x451);  // Ёё
        }


        public static void ValidateTextInRich(RichTextBox r) {

            if (!dictsFound)
                return;

            // Стек, в котором лежат позиции, в которых цвет меняется с красного на фоновый
            LinkedList<int> switches = new LinkedList<int>();

            int l = r.TextLength;
            string t = r.Text;
            int i = 0;
            int lastStoredPos = 0;

            do {
                while (i < l && !PartOfValidableWord(t[i]))
                    i++;

                int wordStart = i;
                while (i < l && PartOfValidableWord(t[i]))
                    i++;

                int wordLength = i - wordStart;
                // Добавляем невалидные слова в стек
                string word = t.Substring(wordStart, wordLength);
                if (word.Length > 0 && !hSpell(word)) {
                    switches.AddLast(wordStart - lastStoredPos);
                    switches.AddLast(wordLength);
                    lastStoredPos = i;
                }

            } while (i < l);

            int ss = r.SelectionStart;
            int sl = r.SelectionLength;

            //tb.SelectAll();
            //tb.SelectionBackColor = BackColor;
            r.Select(0, 0);

            if (switches.Count > 0) {
                LinkedListNode<int> curEl = switches.First;
                bool doRed = true;

                while (curEl != null) {
                    doRed = !doRed;

                    r.SelectionStart += r.SelectionLength;
                    r.SelectionLength = curEl.Value;
                    r.SelectionBackColor = doRed ? Color.Red : r.BackColor;

                    curEl = curEl.Next;
                }
            }

            r.SelectionStart += r.SelectionLength;
            r.SelectionLength = l - r.SelectionStart;
            r.SelectionBackColor = r.Parent.BackColor;

            r.SelectionStart = ss;
            r.SelectionLength = sl;

        }
    }
}
