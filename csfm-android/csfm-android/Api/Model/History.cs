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
using Newtonsoft.Json;

namespace csfm_android.Api.Model
{
    public class History
    {
        [JsonProperty(PropertyName = "TrackId")]
        public int TrackId { get; set; }

        [JsonProperty(PropertyName = "Track")]
        public Track Track { get; set; }

        [JsonProperty(PropertyName = "ListenDate")]
        public DateTime ListenDate { get; set; }

        public bool IsScrobbling { get; set; }

        public History(Track track, DateTime date, bool isScrobbling = false)
        {
            Track = track;
            ListenDate = date;
            IsScrobbling = isScrobbling;
        }

        public History(Track track, bool isScrobbling) : this(track, DateTime.Now, isScrobbling)
        {
        }

    }
}