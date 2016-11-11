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
using csfm_android.Api.Model.Enums;
using Newtonsoft.Json;

namespace csfm_android.Api.Model
{
    public class User
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "Username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "Photo")]
        public string Photo { get; set; }

        [JsonProperty(PropertyName = "Gender")]
        public GenderEnum Gender { get; set; }

    }
}