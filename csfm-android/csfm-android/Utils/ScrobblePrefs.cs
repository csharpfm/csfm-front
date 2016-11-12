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

        public const string ARTIST_KEY = "scrobble.artist";
        public const string ALBUM_KEY = "scrobble.album";
        public const string TRACK_KEY = "scrobble.track";
        public const string END_TICKS_KEY = "scrobble.ticks";

        private static string artist;
        public static string Artist
        {
            get
            {
                if (artist == null) artist = CSFMPrefs.Prefs.GetString(ARTIST_KEY, null);
                return artist;
            }
        }

        private static string album;
        public static string Album
        {
            get
            {
                if (album == null) album = CSFMPrefs.Prefs.GetString(ALBUM_KEY, null);
                return album;
            }
        }

        private static string track;
        public static string Track
        {
            get
            {
                if (track == null) track = CSFMPrefs.Prefs.GetString(TRACK_KEY, null);
                return track;
            }
        }

        private static long? ticks;
        private static long Ticks
        {
            get
            {
                if (!ticks.HasValue || ticks.Value == 0) ticks = CSFMPrefs.Prefs.GetLong(END_TICKS_KEY, 0);
                return ticks.Value;
            }
        }

        public static bool IsSongEnded
        {
            get
            {
                return new DateTime(Ticks) < DateTime.Now;
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
            artist = null;
            editor.PutString(ARTIST_KEY, artist);
            album = null;
            editor.PutString(ALBUM_KEY, album);
            track = null;
            editor.PutString(TRACK_KEY, track);
            ticks = null;
            editor.PutLong(END_TICKS_KEY, endTicks);
            editor.Commit();
        }

        public static void Clear()
        {
            Save(null, null, null, 0);
        }


    }
}