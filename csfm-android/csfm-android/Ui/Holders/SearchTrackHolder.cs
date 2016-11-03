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

namespace csfm_android.Holders
{
    public class SearchTrackHolder : SearchHolder<Track>
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


        public override void Bind(Track item)
        {
            this.Title.Text = item.Name;
            this.Artist.Text = item.Artist_Album_Format;
            this.ImageUrl = item.Album.Image;
            this.Duration.Text = item.Duration_Format;
        }
    }
}