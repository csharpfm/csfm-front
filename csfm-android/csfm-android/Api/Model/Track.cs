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
using csfm_android.Utils;
using Newtonsoft.Json;

namespace csfm_android.Api.Model
{
    /// <summary>
    /// Class representing a song
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Duration of the song
        /// </summary>
        [JsonProperty(PropertyName = "Duration")]
        public long Duration { get; set; }

        /// <summary>
        /// Album Id
        /// </summary>
        [JsonProperty(PropertyName = "AlbumId")]
        public int AlbumId { get; set; }

        /// <summary>
        /// Album the song belongs to
        /// </summary>
        [JsonProperty(PropertyName = "Album")]
        public Album Album { get; set; }

        /// <summary>
        /// Song name
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Song's MuscBrainzId
        /// </summary>
        [JsonProperty(PropertyName = "MbId")]
        public string MbId { get; set; }

        /// <summary>
        /// Song Id
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        [JsonConstructor]
        public Track()
        {

        }
        
        /// <summary>
        /// Returns '{artist} - {album}' (or '{artist}' in case of an empty album name.
        /// </summary>
        public string Artist_Album_Format
        {
            get
            {
                string text = Album?.Artist?.Name.DefaultStringIfEmpty("Unknown Artist");
                if (!(Album?.Name).IsStringEmpty())
                {
                    text += " - " + Album.Name;
                }
                return text;
            }
        }

        /// <summary>
        /// Returns the duration in 00:00 format (mm:ss)
        /// </summary>
        public string Duration_Format
        {
            get
            {   
                if (Duration > 0)
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(Duration);
                    return string.Format("{0:00}:{1:00}", (int)timeSpan.TotalMinutes, (int)timeSpan.Seconds);
                    
                }
                return string.Empty;
            }
        }
    }
}