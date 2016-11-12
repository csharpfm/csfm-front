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
    public class SearchArtistHolder : SearchHolder<History>
    {
        public const int LAYOUT = Resource.Layout.search_artist_item;
        public TextView Title { get; set; }
        public ImageView Image { get; private set; }

        private string ImageUrl
        {
            set
            {
                if (value != null)
                    Picasso.With(Application.Context)
                       .Load(value)
                       .Transform(new CircleTransform())
                       .Placeholder(Resource.Drawable.ic_music_circle_grey600_24dp)
                       .Into(Image);
                else
                {
                    Picasso.With(Application.Context)
                        .Load(Resource.Drawable.ic_music_circle_grey600_24dp)
                        .Transform(new CircleTransform())
                        .Into(Image);
                }
            }
        }

        public SearchArtistHolder(View itemView) : base(itemView)
        {
            this.Title = itemView.FindViewById<TextView>(Resource.Id.search_artist_name);
            this.Image = itemView.FindViewById<ImageView>(Resource.Id.search_image);
        }

        public override void Bind(History item)
        {
            this.Title.Text = item.Track.Album.Artist.Name;
            this.ImageUrl = item.Track.Album.Artist.Image != null ? item.Track.Album.Artist.Image : (item.Track.Album.Image != null ? item.Track.Album.Image : item.Track.Album.Artist.Albums.FirstOrDefault(a => a.Image != null)?.Image);
        }
    }
}