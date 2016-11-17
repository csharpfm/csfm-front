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
using static Android.Provider.MediaStore.Audio;
using Android.Database;

namespace csfm_android.Utils
{
    /// <summary>
    /// Manage metadata from the internally stored music files
    /// </summary>
    public class MusicLibrary
    {
        /// <summary>
        /// Query the MediaStore database to get the album art for a specified artist and album. Can potentially return multiple results in case of duplicate (artist, album) couple.
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAlbumArt(string artist, string album, Context context) //Get Album Art from MediaStore if available
        {
            string condition = string.Format("{0} = ? AND {1} = ?",
                 AlbumColumns.Artist,
                 AlbumColumns.Album
                );

            ICursor c = context.ContentResolver.Query(
                MediaStore.Audio.Albums.ExternalContentUri,
                new string[] { AudioColumns.AlbumArt },
                condition, new string[] { artist, album },
                Albums.DefaultSortOrder
                );

            List<string> arts = new List<string>(c.Count);

            if (c.MoveToFirst())
            {
                do
                {
                    arts.Add(c.GetString(c.GetColumnIndex(AudioColumns.AlbumArt)));
                }
                while (c.MoveToNext());
            }

            return arts.Distinct();

        }
    }
}