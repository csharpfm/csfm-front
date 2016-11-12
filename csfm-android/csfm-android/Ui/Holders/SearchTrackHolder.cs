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
using csfm_android.Api.Model;
using Square.Picasso;
using csfm_android.Ui.Utils;

namespace csfm_android.Ui.Holders
{
    public class SearchTrackHolder : SearchHolder<History>
    {
        public const int LAYOUT = Resource.Layout.search_track_item;

        public TextView Title { get; set; }
        public TextView Artist { get; set; }
        public TextView Duration { get; set; }
        public ImageView Image { get; private set; }

        private string ImageUrl
        {
            set
            {
                Picasso.With(Application.Context)
                       .Load(value)
                       .Transform(new CircleTransform())
                       .Into(Image);
            }
        }

        public SearchTrackHolder(View itemView) : base(itemView)
        {
            this.Title = itemView.FindViewById<TextView>(Resource.Id.search_track_name);
            this.Artist = itemView.FindViewById<TextView>(Resource.Id.search_track_artist);
            this.Image = itemView.FindViewById<ImageView>(Resource.Id.search_track_image);
            this.Duration = itemView.FindViewById<TextView>(Resource.Id.search_track_duration);
        }


        public override void Bind(History item)
        {
            this.Title.Text = item.Track.Name;
            this.Artist.Text = item.Track.Artist_Album_Format;
            this.Duration.Text = item.Track.Duration_Format;
            this.ImageUrl = item.Track.Album.Image != null ? item.Track.Album.Image : item.Track.Album.Image != null ? item.Track.Album.Image : item.Track.Album.Artist.Albums.FirstOrDefault(a => a.Image != null)?.Image;

        }
    }
}