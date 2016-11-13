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
    public class Album
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "MbId")]
        public string MbId { get; set; }

        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty(PropertyName = "ArtistId")]
        public int ArtistId { get; set; }

        [JsonProperty(PropertyName = "Artist")]
        public Artist Artist { get; set; }

        [JsonProperty(PropertyName = "Tracks", IsReference = true)]
        public List<Track> Tracks { get; set; }

        [JsonConstructor]
        public Album()
        {

        }
    }
}