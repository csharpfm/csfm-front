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
    /// <summary>
    /// Class representing a user
    /// </summary>
    public class User
    {
        /// <summary>
        /// User Id
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// User's Email
        /// </summary>
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// User's username
        /// </summary>
        [JsonProperty(PropertyName = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// User's profile picture
        /// </summary>
        [JsonProperty(PropertyName = "Photo")]
        public string Photo { get; set; }

        /// <summary>
        /// User's gender
        /// </summary>
        [JsonProperty(PropertyName = "Gender")]
        public GenderEnum Gender { get; set; }

    }
}