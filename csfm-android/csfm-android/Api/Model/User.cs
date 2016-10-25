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
    public class User
    {
        public string Username { get; set; }

        public string Lastname { get; set; }

        public string Firstname { get; set; }

        public DateTime Birth { get; set; }

        public List<Track> History { get; set; }

        public List<User> Matches { get; set; }

        public string Image { get; set; }

        public string Email { get; set; }



    }
}