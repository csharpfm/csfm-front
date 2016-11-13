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

namespace csfm_android.Utils.iTunes
{
    public class ITunesResponseItem
    {
        [JsonProperty(PropertyName = "artistName")]
        public string ArtistName { get; set; }

        [JsonProperty(PropertyName = "collectionName")]
        public string AlbumName { get; set; }

        [JsonProperty(PropertyName = "artworkUrl100")]
        public string AlbumArtUrl { get; set; }
    }
}