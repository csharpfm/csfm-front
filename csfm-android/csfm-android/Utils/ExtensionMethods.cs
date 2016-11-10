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
    public static class ExtensionMethods
    {
        public static bool IsStringEmpty(this String text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static string DefaultStringIfEmpty(this String text, string defaultText)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return defaultText;
            }
            return text;
        }

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

        public static Java.Lang.Object ToJavaObject<TObject>(this TObject value)
        {
            if (Equals(value, default(TObject)) && !typeof(TObject).IsValueType)
                return null;

            var holder = new JavaHolder(value);

            return holder;
        }

        public static IEnumerable<string> ToTrackNames(this List<History> history)
        {
            return history.Select(h => h.Track.Name);
        }

        public static IEnumerable<string> ToAlbumNames(this List<History> history)
        {
            return history.Select(h => h.Track.Album.Name);
        }

        public static IEnumerable<string> ToArtistNames(this List<History> history)
        {
            return history.Select(h => h.Track.Artist.Name);
        }

        public static string GetArtist(this Intent intent)
        {
            return intent.GetStringExtra(MediaStore.Audio.AudioColumns.Artist);
        }

        public static string GetAlbum(this Intent intent)
        {
            return intent.GetStringExtra(MediaStore.Audio.AudioColumns.Album);
        }

        public static string GetTrack(this Intent intent)
        {
            return intent.GetStringExtra(MediaStore.Audio.AudioColumns.Track);
        }

        public static long GetDuration(this Intent intent, long defaultValue)
        {
            return intent.GetLongExtra(MediaStore.Audio.AudioColumns.Duration, defaultValue);
        }

        public static long GetPosition(this Intent intent, long defaultValue)
        {
            return intent.GetLongExtra("position", defaultValue);
        }

        public static void AddSong(this Bundle extras, string artist, string album, string track)
        {
            if (!artist.IsStringEmpty())
                extras.PutString(MediaStore.Audio.AudioColumns.Artist, artist);

            if (!album.IsStringEmpty())
                extras.PutString(MediaStore.Audio.AudioColumns.Album, album);

            if (!track.IsStringEmpty())
                extras.PutString(MediaStore.Audio.AudioColumns.Track, track);
        }

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
                Artist = artist
            };

            artist.Albums = new List<Album> { album };
            album.Tracks = new List<Track> { track };


            return new History(track, true);


        }

        public static IntentFilter AddActions(this IntentFilter intentFilter, params string[] actions)
        {
            foreach(string a in actions)
            {
                intentFilter.AddAction(a);
            }

            return intentFilter;
        }

    }


    public class JavaHolder : Java.Lang.Object
    {
        public readonly object Instance;

        public JavaHolder(object instance)
        {
            Instance = instance;
        }
    }


}