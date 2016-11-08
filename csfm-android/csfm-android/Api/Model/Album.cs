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
    public class Album : MusicItem
    {
        public string Name { get; set; }

        public string MbId { get; set; }

        public string Image { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Artist Artist { get; set; }

        public List<Track> Tracks { get; set; }

    }
}