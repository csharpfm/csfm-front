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
    public class Artist : MusicItem
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "MbId")]
        public string MbId { get; set; }

        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "Albums")]
        public List<Album> Albums { get; set; }

    }
}