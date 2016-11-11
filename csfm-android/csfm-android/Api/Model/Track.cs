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
    public class Track : MusicItem
    {

        [JsonProperty(PropertyName = "Duration")]
        public long Duration { get; set; }

        [JsonProperty(PropertyName = "Album")]
        public Album Album { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "MbId")]
        public string MbId { get; set; }

        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        public string Artist_Album_Format
        {
            get
            {
                string text = Album.Artist.Name.DefaultStringIfEmpty("Unknown Artist");
                if (!Album.Name.IsStringEmpty())
                {
                    text += " - " + Album.Name;
                }
                return text;
            }
        }

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

        /*  public List<TrackTag> Tags { get; set; } */
    }
}