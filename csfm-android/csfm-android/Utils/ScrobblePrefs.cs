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
using Android.Media;

namespace csfm_android.Utils
{
    public class ScrobblePrefs
    {

        public const string ARTIST_KEY = "artist";
        public const string ALBUM_KEY = "album";
        public const string TRACK_KEY = "track";
        public const string END_TICKS_KEY = "ticks";

        public static string Artist
        {
            get
            {
                return CSFMPrefs.Prefs.GetString(ARTIST_KEY, null);
            }
        }

        public static string Album
        {
            get
            {
                return CSFMPrefs.Prefs.GetString(ALBUM_KEY, null);
            }
        }

        public static string Track
        {
            get
            {
                return CSFMPrefs.Prefs.GetString(TRACK_KEY, null);
            }
        }

        public static bool IsSongEnded
        {
            get
            {
                DateTime test = new DateTime(CSFMPrefs.Prefs.GetLong(END_TICKS_KEY, 0));
                return new DateTime(CSFMPrefs.Prefs.GetLong(END_TICKS_KEY, 0)) < DateTime.Now;
            }
        }

        public static bool HasValue
        {
            get
            {
                return Artist != null && Album != null && Track != null;
            }
        }

        private static AudioManager audioManager = null;
        public static bool IsPlaying
        {
            get
            {
                if (audioManager == null)
                {
                    audioManager = (AudioManager) Application.Context.GetSystemService(Context.AudioService);
                }
                return audioManager.IsMusicActive;
            }
        }



        public static void Save(string artist, string album, string track, long endTicks)
        {
            var editor = CSFMPrefs.Editor;
            editor.PutString(ARTIST_KEY, artist);
            editor.PutString(ALBUM_KEY, album);
            editor.PutString(TRACK_KEY, track);
            editor.PutLong(END_TICKS_KEY, endTicks);
            editor.Commit();
        }

        public static void Clear()
        {
            Save(null, null, null, 0);
        }


    }
}