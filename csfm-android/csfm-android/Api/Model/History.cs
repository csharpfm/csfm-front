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
    /// History item : Track played at a given time
    /// </summary>
    public class History
    {
        /// <summary>
        /// Id of the history item
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Track Id
        /// </summary>
        [JsonProperty(PropertyName = "TrackId")]
        public int TrackId { get; set; }

        /// <summary>
        /// The scrobbled track
        /// </summary>
        [JsonProperty(PropertyName = "Track")]
        public Track Track { get; set; }

        /// <summary>
        /// The date the song was played
        /// </summary>
        [JsonProperty(PropertyName = "ListenDate")]
        public DateTime ListenDate { get; set; }

        /// <summary>
        /// Indicates whether the song is still being listened to or if the scrobble has ended.
        /// </summary>
        [JsonIgnore]
        public bool IsScrobbling { get; set; }

        /// <summary>
        /// Get the formatted listen date
        /// </summary>
        [JsonIgnore]
        public string ListenDateFormat
        {
            get
            {
                return ListenDate.ToString();
            }
        }

        /// <summary>
        /// Get the album image if defined, otherwise the artist image if defined, or null.
        /// </summary>
        [JsonIgnore]
        public string Image
        {
            get
            {
                return Track?.Album?.Image != null ? Track.Album.Image : Track?.Album?.Artist?.Image;
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        [JsonConstructor]
        public History()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="track">The track</param>
        /// <param name="date">The listen date</param>
        /// <param name="isScrobbling">Whether the song is still scrobbling or not</param>
        public History(Track track, DateTime date, bool isScrobbling = false)
        {
            Track = track;
            ListenDate = date;
            IsScrobbling = isScrobbling;
        }

        /// <summary>
        /// Specific constructor in case of a newly played song (the listen date will be the time the constructor was called at).
        /// </summary>
        /// <param name="track">The track</param>
        /// <param name="isScrobbling">Whether the song is still scrobbling or not</param>
        public History(Track track, bool isScrobbling) : this(track, DateTime.Now, isScrobbling)
        {
        }

        /// <summary>
        /// Returns true if the track name, album name, and artist name are the same
        /// </summary>
        /// <param name="other">Track to compare with</param>
        /// <returns></returns>
        public bool IsSameTrack(Track other)
        {
            Track track = this.Track;
            return track != null && other != null && track.Name == other.Name && track.Album?.Name == other.Album?.Name && track.Album?.Artist?.Name == other.Album?.Artist?.Name;
        }

    }
}