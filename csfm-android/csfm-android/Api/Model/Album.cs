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
    /// Music album
    /// </summary>
    public class Album
    {
        /// <summary>
        /// Album Id
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Album title
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Album's MusicBrainzId
        /// </summary>
        [JsonProperty(PropertyName = "MbId")]
        public string MbId { get; set; }

        /// <summary>
        /// Album Art Url
        /// </summary>
        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }

        /// <summary>
        /// Release date of the album
        /// </summary>
        [JsonProperty(PropertyName = "ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Id of the artist
        /// </summary>
        [JsonProperty(PropertyName = "ArtistId")]
        public int ArtistId { get; set; }

        /// <summary>
        /// Album's artist
        /// </summary>
        [JsonProperty(PropertyName = "Artist")]
        public Artist Artist { get; set; }

        /// <summary>
        /// Album's tracks
        /// </summary>
        [JsonProperty(PropertyName = "Tracks", IsReference = true)]
        public List<Track> Tracks { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        [JsonConstructor]
        public Album()
        {

        }
    }
}