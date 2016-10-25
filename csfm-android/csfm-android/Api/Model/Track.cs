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

namespace csfm_android.Api.Model
{
    public class Track
    {

        public string Name { get; set; }

        public string MbId { get; set; }

        public int Duration { get; set; }

        public Album Album { get; set; }

        public Artist Artist { get; set; }

        /*  public List<TrackTag> Tags { get; set; } */
    }
}