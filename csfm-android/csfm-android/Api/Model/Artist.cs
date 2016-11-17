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
    /// <summary>
    /// Artist or band
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// Artist Id
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Aritst or band name
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Artist's MusicBrainzId
        /// </summary>
        [JsonProperty(PropertyName = "MbId")]
        public string MbId { get; set; }

        /// <summary>
        /// Artist Image Url
        /// </summary>
        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }

        /// <summary>
        /// Albums of the artist
        /// </summary>
        [JsonProperty(PropertyName = "Albums", IsReference = true)]
        public List<Album> Albums { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        [JsonConstructor]
        public Artist()
        {

        }
    }
}