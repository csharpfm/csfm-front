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
using csfm_android.Api.Model;
using Android.Provider;

namespace csfm_android.Utils
{
    /// <summary>
    /// Extension methods used in the projet
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks whether a string is empty/null (true) or not (false)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsStringEmpty(this String text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Replaces the text by a default text if empty/null.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public static string DefaultStringIfEmpty(this String text, string defaultText)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return defaultText;
            }
            return text;
        }

        /// <summary>
        /// Capitalize the first letter of the text (hello world test --> Hello world test)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToFirstUpperCase(this String text)
        {
            if (text != null)
            {
                if (text.Length > 1)
                {
                    return text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1);
                }
                else
                {
                    return text.ToUpper();
                }
            }
            return text;

        }

        /// <summary>
        /// Capitalize first letter of each word (hello world test --> Hello World Test)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToFirstUpperCases(this String text)
        {
            string[] texts = text.Split(' ');
            string result = "";
            for (int i = 0; i < texts.Length; i++)
            {
                if (i > 0) result += " ";
                result += texts[i].ToFirstUpperCase();
            }
            return result;
        }

        /// <summary>
        /// Special Java --> .Net deserializer
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TObject ToNetObject<TObject>(this Java.Lang.Object value)
        {
            if (value == null)
                return default(TObject);

            if (!(value is JavaHolder))
                throw new InvalidOperationException("Unable to convert to .NET object. Only Java.Lang.Object created with .ToJavaObject() can be converted.");

            TObject returnVal;
            try { returnVal = (TObject)((JavaHolder)value).Instance; }
            finally { value.Dispose(); }
            return returnVal;
        }

        /// <summary>
        /// Special .Net --> Java serializer
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Java.Lang.Object ToJavaObject<TObject>(this TObject value)
        {
            if (Equals(value, default(TObject)) && !typeof(TObject).IsValueType)
                return null;

            var holder = new JavaHolder(value);

            return holder;
        }

        /// <summary>
        /// Get a list of track names from a list of history items
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToTrackNames(this List<History> history)
        {
            return history.Select(h => h.Track.Name);
        }

        /// <summary>
        /// Get a list of album names from a list of history items
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToAlbumNames(this List<History> history)
        {
            return history.Select(h => h.Track.Album.Name);
        }

        /// <summary>
        /// Get a list of artist names from a list of history items
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToArtistNames(this List<History> history)
        {
            return history.Select(h => h.Track.Album.Artist.Name);
        }

        /// <summary>
        /// Get the artist name from the broadcast intents
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public static string GetArtist(this Intent intent)
        {
            return intent.GetStringExtra(MediaStore.Audio.AudioColumns.Artist);
        }

        /// <summary>
        /// Get the album name from the broadcast intents
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public static string GetAlbum(this Intent intent)
        {
            return intent.GetStringExtra(MediaStore.Audio.AudioColumns.Album);
        }

        /// <summary>
        /// Get the track name from the broadcast intents
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public static string GetTrack(this Intent intent)
        {
            return intent.GetStringExtra(MediaStore.Audio.AudioColumns.Track);
        }

        /// <summary>
        /// Get the duration from the broadcast intent (or default value if not specified)
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetDuration(this Intent intent, long defaultValue)
        {
            return intent.GetLongExtra(Configuration.Music.EXTRA_DURATION, defaultValue);
        }

        /// <summary>
        /// Get the song position (in the total song duration) or default value if not specified
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetPosition(this Intent intent, long defaultValue)
        {
            return intent.GetLongExtra(Configuration.Music.EXTRA_POSITION, defaultValue);
        }

        /// <summary>
        /// Add artist, album, track names and left duration (duration - position) to a bundle
        /// </summary>
        /// <param name="extras"></param>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="track"></param>
        /// <param name="endTicks">Ticks left till the end of the song</param>
        public static void AddSong(this Bundle extras, string artist, string album, string track, long endTicks)
        {
            if (!artist.IsStringEmpty())
                extras.PutString(Configuration.Music.EXTRA_ARTIST, artist);

            if (!album.IsStringEmpty())
                extras.PutString(Configuration.Music.EXTRA_ALBUM, album);

            if (!track.IsStringEmpty())
                extras.PutString(Configuration.Music.EXTRA_TRACK, track);

            if (endTicks > 0)
                extras.PutLong(Configuration.Music.EXTRA_TICKS_TO_END, endTicks);
        }

        /// <summary>
        /// Get the history item from an intent
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="isScrobbling"></param>
        /// <returns></returns>
        public static History ToHistoryItem(this Intent intent, bool isScrobbling)
        {
            string artistString = intent.GetArtist();
            string albumString = intent.GetAlbum();
            string trackString = intent.GetTrack();
            Artist artist = new Artist
            {
                Name = artistString
            };

            Album album = new Album
            {
                Name = albumString,
                Artist = artist,
                Image = MusicLibrary.GetAlbumArt(artistString, albumString, Application.Context).FirstOrDefault(),
            };

            Track track = new Track
            {
                Name = trackString,
                Album = album,
            };

            artist.Albums = new List<Album> { album };
            album.Tracks = new List<Track> { track };


            return new History(track, true);


        }

        /// <summary>
        /// Add multiple actions at once in an intent filter
        /// </summary>
        /// <param name="intentFilter"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        public static IntentFilter AddActions(this IntentFilter intentFilter, params string[] actions)
        {
            foreach(string a in actions)
            {
                intentFilter.AddAction(a);
            }

            return intentFilter;
        }

    }

    /// <summary>
    /// Java Object Holder
    /// </summary>
    public class JavaHolder : Java.Lang.Object
    {
        public readonly object Instance;

        public JavaHolder(object instance)
        {
            Instance = instance;
        }
    }


}