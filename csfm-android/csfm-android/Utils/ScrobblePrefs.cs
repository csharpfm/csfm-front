using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace csfm_android.Utils
{
    public class ScrobblePrefs
    {
        public const string ARTIST_KEY = "artist";
        public const string ALBUM_KEY = "album";
        public const string TRACK_KEY = "track";
        public const string IS_PLAYING_KEY = "isPlaying";

        public static bool IsPlaying
        {
            get
            {
                return CSFMPrefs.Prefs.GetBoolean(IS_PLAYING_KEY, false);
            }

            set
            {
                CSFMPrefs.Editor.PutBoolean(IS_PLAYING_KEY, value).Commit();
            }
        }

        public static void Save(string artist, string album, string track)
        {
            var editor = CSFMPrefs.Editor;
            editor.PutString(ARTIST_KEY, artist);
            editor.PutString(ALBUM_KEY, album);
            editor.PutString(TRACK_KEY, track);
            editor.PutBoolean(IS_PLAYING_KEY, true);
            editor.Commit();
        }

        public static void Clear()
        {
            IsPlaying = false;
        }


    }
}