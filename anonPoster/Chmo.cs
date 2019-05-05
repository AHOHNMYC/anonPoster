using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;

#if USE_CHMO
namespace anonPoster {
    class Chmo {
        
        private LinkedList<string> preTree;
        private Dictionary<char, object> bakedTree;


        private Dictionary<char, object> LoadTree(int depth = 3) {
            // Считываем слой дерева
            string leafs = preTree.First.Value;
            preTree.RemoveFirst();

            Dictionary<char, object> subTree = new Dictionary<char, object>();
            for (int i = 0; i < leafs.Length; i++)
                subTree[leafs[i]] = (depth > 0) ? LoadTree(depth - 1) : null;
            return subTree;
        }


        public Chmo() {
            byte[] chmoPackedTreeBunpacked = new byte[19037];
            new GZipStream(new MemoryStream(Properties.Resources.chmoPackedTree), CompressionMode.Decompress).Read(chmoPackedTreeBunpacked, 0, 19037);

            #region Распаковка Win-1251
            Encoding win1251 = Encoding.GetEncoding("windows-1251");
            Encoding unicode = Encoding.Unicode;

            byte[] unicodeBytes = Encoding.Convert(win1251, unicode, chmoPackedTreeBunpacked);
            string packedTree = unicode.GetString(unicodeBytes);
            #endregion

            preTree = new LinkedList<string>(packedTree.Split('|'));

            bakedTree = LoadTree();
        }


#region Переработанный
        /// <summary>
        /// Возвращает массив странных строк, не прошедших валидацию
        /// </summary>
        /// <remarks>
        /// Валидация — проверка первых 4-х символов слова по цепи Маркова
        /// </remarks>
        /// <param name="input">
        /// Строка для проверки
        /// </param>
        public string[] CheckString(string input) {

            input = input.ToLower();

            List<string> strangeWords = new List<string>();
            int position = 0;

            do {
                Dictionary<char, object> partOfTree = bakedTree;

                // Считываем нерусские символы
                while (position < input.Length && !bakedTree.ContainsKey(input[position]))
                    position++;

                int validSymbols = 0;
                int wordStart = position;
                // Считываем максимальное количество символов по дереву
                while (position < input.Length && partOfTree != null && partOfTree.ContainsKey(input[position])) {
                    partOfTree = partOfTree[input[position]] as Dictionary<char, object>;
                    validSymbols++;
                    position++;
                }

                // Дочитываем остальную кириллицу
                while (position < input.Length && bakedTree.ContainsKey(input[position]))
                    position++;

                int wordLength = position - wordStart;
                // Если у нас меньше четырёх валидных символов, нам прислали хуйню, отправляем обратно
                if (validSymbols < 4  && validSymbols < wordLength)
                    strangeWords.Add(input.Substring(wordStart, wordLength));

            } while (position < input.Length);

            return strangeWords.ToArray();
        }
#endregion


#region По Ычу, оптимизированный
        /*
        public bool CheckString(string input, ref string[] output) {

            List<string> strangeWords = new List<string>();
            Dictionary<char, object> partOfTree = bakedTree;
            int curLetterNumber = 0;

            input = input.ToLower();

            for (int position = 0; position < input.Length; position++) {
                char currentChar = input[position];

                if (bakedTree.ContainsKey(currentChar)) {
                    if (partOfTree.ContainsKey(currentChar)) {
                        curLetterNumber++;
                        if (curLetterNumber < 4) {
                            partOfTree = partOfTree[currentChar] as Dictionary<char, object>;
                            continue;

                        } else {
                            while (position < input.Length && bakedTree.ContainsKey(input[position]))
                                position++;
                            partOfTree = bakedTree;
                        }
                    } else {
                        while (position < input.Length && bakedTree.ContainsKey(input[position])) {
                            position++;
                            curLetterNumber++;
                        }
                        strangeWords.Add(input.Substring(position - curLetterNumber, curLetterNumber));

                        partOfTree = bakedTree;
                    }
                } else {
                    partOfTree = bakedTree;

                }
                curLetterNumber = 0;
            }

            output = strangeWords.ToArray();
            return 0 == output.Length;
        }
        */
#endregion


        #region По Ычу
        /*
        public bool CheckString(string input, ref string[] output) {

            List<string> strangeWords = new List<string>();
            Dictionary<char, object> partOfTree = bakedTree;
            int curLetterNumber = 0;
            int startOfStrangeWord = 0;

            input = input.ToLower();

            for (int i = 0; i < input.Length; i++) {
                char currentChar = input[i];

                // If we find symbol out of dictionary
                if (!bakedTree.ContainsKey(currentChar)) {
                    partOfTree = bakedTree;
                    startOfStrangeWord = i + 1;
                    continue;
                }

                // If we find symbol that exists in out tree
                if (partOfTree.ContainsKey(currentChar)) {
                    curLetterNumber++;
                    if (curLetterNumber < 4) {
                        partOfTree = partOfTree[currentChar] as Dictionary<char, object>;
                    } else {
                        while (i < input.Length && bakedTree.ContainsKey(input[i]))
                            i++;
                        curLetterNumber = 0;
                        partOfTree = bakedTree;
                        startOfStrangeWord = i + 1;
                    }
                } else {
                    if (bakedTree.ContainsKey(currentChar)) {
                        while (i < input.Length && bakedTree.ContainsKey(input[i]))
                            i++;
                        strangeWords.Add(input.Substring(startOfStrangeWord, i - startOfStrangeWord));
                    }
                    startOfStrangeWord = i + 1;
                    partOfTree = bakedTree;
                    curLetterNumber = 0;
                }
            }

            output = strangeWords.ToArray();
            return 0 == strangeWords.Count;
        }
        */
        #endregion


    }
}
#endif
