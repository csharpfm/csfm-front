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
    public class ITunesResponse
    {
        [JsonProperty(PropertyName = "resultCount")]
        public string Count
        {
            get; set;
        }

        [JsonProperty(PropertyName = "results")]
        public List<ITunesResponseItem> Items { get; set; }
    }

}