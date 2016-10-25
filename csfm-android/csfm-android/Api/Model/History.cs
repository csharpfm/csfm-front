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
    public class History
    {
        public Track Track { get; set; }

        public DateTime Date { get; set; }

        public History(Track track, DateTime date)
        {
            Track = track;
            Date = date;
        }

    }
}