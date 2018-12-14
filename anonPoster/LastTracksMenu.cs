using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace anonPoster {
    public class LastTracksMenu {

        class Track {
            public DateTime Time;
            public string Title;
        }

        LinkedList<Track> tracks = new LinkedList<Track>();
        public ContextMenu menu = new ContextMenu();
        
        private void OpenGoogleTrackSearch(object s, EventArgs e) => Process.Start(URLs.Googel((s as MenuItem).Tag as string));
        private MainForm mf;

        public LastTracksMenu(MainForm _mf) {
            mf = _mf;
        }

        /// <summary>
        /// Tries to add track to context menu
        /// </summary>
        /// <returns>Is tracks already exists in menu</returns>
        public bool UpdateCoverMenu(DateTime newTime, string newTitle) {

            // Handle new track
            if (tracks.First == null) {
                menu.MenuItems.Add("-");
                menu.MenuItems.Add("Форсировать запрос тегов", (s, e) => mf.rw.TestRadio());
                menu.MenuItems.Add("Попробовать скачать =>", (s, e) => Process.Start(URLs.radioSong));
            } else {
                if (newTitle == tracks.First.Value.Title)
                    return false;
            }


            tracks.AddFirst(new Track { Time = newTime, Title = newTitle });

            if (tracks.Count > 10) {
                tracks.RemoveLast();
                menu.MenuItems.RemoveAt(menu.MenuItems.Count - 3); // Remove 10-th track
            }

            menu.MenuItems.Add(0, new MenuItem($"{newTime.Hour}:{newTime.Minute} : {newTitle}", OpenGoogleTrackSearch) { Tag = newTitle });

            return true;
        }
    }
}
