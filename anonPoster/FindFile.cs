using System.IO;
using static System.Environment;

namespace anonPoster {
    static class FindFile {
        public static string PathFindOnPath(string file) {
            return PathFindOnPath(file, (string[])null);
        }
        public static string PathFindOnPath(string[] files) {
            foreach (string file in files) {
                string t = PathFindOnPath(file);
                if (t != null)
                    return t;
            }
            return "";
        }

        public static string PathFindOnPath(string file, string otherDirs) {
            return PathFindOnPath(file, new string[] { otherDirs });
        }
        public static string PathFindOnPath(string[] file, string otherDirs) {
            return PathFindOnPath(file, new string[] { otherDirs });
        }

        public static string PathFindOnPath(string[] files, string[] otherDirs) {
            foreach (string file in files) {
                string t = PathFindOnPath(file, otherDirs);
                if (t != null)
                    return t;
            }
            return "";
        }
        public static string PathFindOnPath(string file, string[] otherDirs) {
            string azazazaz = $"{Directory.GetCurrentDirectory()}\\{file}";
            if (File.Exists(azazazaz)) return azazazaz;

            string res;

            if (otherDirs != null) {
                res = _retFile(file, otherDirs);
                if (res != null) return res;
            }

            return _retFile(file, GetEnvironmentVariable("path").Split(';'));
        }
        private static string _retFile(string file, string[] otherDirs) {
            foreach (string path in otherDirs) {
                string filePath = path;
                if (!filePath.EndsWith("\\")) filePath += '\\';
                filePath += file;
                if (File.Exists(filePath)) return filePath;
            }
            return null;
        }
    }
}
