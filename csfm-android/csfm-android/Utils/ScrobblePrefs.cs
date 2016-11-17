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
    /// <summary>
    /// Extension to the CSFMPrefs class : Store values into internal storage dedicated to scrobbling
    /// </summary>
    public class ScrobblePrefs
    {

        public const string ARTIST_KEY = "scrobble.artist";
        public const string ALBUM_KEY = "scrobble.album";
        public const string TRACK_KEY = "scrobble.track";
        public const string END_TICKS_KEY = "scrobble.ticks";

        private static string artist;

        /// <summary>
        /// Get the scrobbling artist
        /// </summary>
        public static string Artist
        {
            get
            {
                if (artist == null) artist = CSFMPrefs.Prefs.GetString(ARTIST_KEY, null);
                return artist;
            }
        }

        private static string album;

        /// <summary>
        /// Get the scrobbling album
        /// </summary>
        public static string Album
        {
            get
            {
                if (album == null) album = CSFMPrefs.Prefs.GetString(ALBUM_KEY, null);
                return album;
            }
        }

        private static string track;

        /// <summary>
        /// Get the scrobbling track
        /// </summary>
        public static string Track
        {
            get
            {
                if (track == null) track = CSFMPrefs.Prefs.GetString(TRACK_KEY, null);
                return track;
            }
        }

        private static long? ticks;

        /// <summary>
        /// Get the ticks representing the end of the song datetime
        /// </summary>
        private static long Ticks
        {
            get
            {
                if (!ticks.HasValue || ticks.Value == 0) ticks = CSFMPrefs.Prefs.GetLong(END_TICKS_KEY, 0);
                return ticks.Value;
            }
        }

        /// <summary>
        /// Returns whether the song has ended (if the user killed the service and didn't hit pause) based on the ticks.
        /// </summary>
        public static bool IsSongEnded
        {
            get
            {
                return new DateTime(Ticks) < DateTime.Now;
            }
        }

        /// <summary>
        /// Whether a scrobble is currently stored in memory
        /// </summary>
        public static bool HasValue
        {
            get
            {
                return Artist != null && Album != null && Track != null;
            }
        }

        /// <summary>
        /// Util to check if a song is currently playing. This property does NOT use SharedPreferences, but AudioManager (from Android AudioService)
        /// </summary>
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


        /// <summary>
        /// Save a new scrobble into memory
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="track"></param>
        /// <param name="endTicks"></param>
        public static void Save(string artist, string album, string track, long endTicks)
        {
            var editor = CSFMPrefs.Editor;
            ScrobblePrefs.artist = null;
            editor.PutString(ARTIST_KEY, artist);
            ScrobblePrefs.album = null;
            editor.PutString(ALBUM_KEY, album);
            ScrobblePrefs.track = null;
            editor.PutString(TRACK_KEY, track);
            ScrobblePrefs.ticks = null;
            editor.PutLong(END_TICKS_KEY, endTicks);
            editor.Commit();
        }

        /// <summary>
        /// Clear memory
        /// </summary>
        public static void Clear()
        {
            Save(null, null, null, 0);
        }


    }
}