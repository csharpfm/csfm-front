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
using Android.Provider;

namespace csfm_android
{
    /// <summary>
    /// Constant configuration values
    /// </summary>
    public class Configuration
    {
        public const string LABEL = "MatchFM";
        public const string LOGIN_THEME = "@style/LogTheme";
        public const string MAIN_THEME = "@style/MyTheme";
        public const string ICON = "@drawable/icon";
        

        public class Music
        {
            public const string ACTION_METACHANGED = "com.android.music.metachanged";
            public const string ACTION_PLAYSTATECHANGED = "com.android.music.playstatechanged";
            public const string ACTION_PLAYBACKCOMPLETE = "com.android.music.playbackcomplete";
            public const string ACTION_QUEUECHANGED = "com.android.music.queuechanged";
            public static readonly string[] MUSIC_ACTIONS = { ACTION_METACHANGED, ACTION_PLAYBACKCOMPLETE, ACTION_PLAYSTATECHANGED, ACTION_QUEUECHANGED };

            public const string EXTRA_PLAYING = "playing";
            public static readonly string EXTRA_ARTIST = MediaStore.Audio.AudioColumns.Artist;
            public static readonly string EXTRA_ALBUM = MediaStore.Audio.AudioColumns.Album;
            public static readonly string EXTRA_TRACK = MediaStore.Audio.AudioColumns.Track;
            public const string EXTRA_POSITION = "position";
            public static readonly string EXTRA_DURATION = MediaStore.Audio.AudioColumns.Duration;
            public const string EXTRA_TICKS_TO_END = "ticks";
        }

        public class Location
        {
            public const int MIN_TIME = 2000;
            public const int MIN_DISTANCE = 1000;
            public const string LOCATION_PROVIDER = "network";
        }

        public class BottomBar
        {
            public const string BOTTOM_BAR_BACKGROUND_COLOR = "#F44336";
            public const string INACTIVE_ICON_COLOR = "#44000000";
        }

        
    }
}