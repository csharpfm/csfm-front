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
using csfm_android.Ui.Utils;
using Square.Picasso;

namespace csfm_android.Ui.Holders
{
    public class SearchAlbumHolder : SearchHolder<Album>
    {
        public const int LAYOUT = Resource.Layout.search_album_item;
        public TextView Title { get; private set; }
        public TextView Artist { get; private set; }

        public ImageView Image { get; private set; }
        private string ImageUrl
        {
            set
            {
                Picasso.With(Application.Context)
                       .Load(value)
                       //.Transform(new CircleTransform())
                       .Into(Image);
            }
        }


        public SearchAlbumHolder(View itemView) : base(itemView)
        {
            this.Title = itemView.FindViewById<TextView>(Resource.Id.search_album_name);
            this.Artist = itemView.FindViewById<TextView>(Resource.Id.search_album_artist);
            this.Image = itemView.FindViewById<ImageView>(Resource.Id.search_image);
        }

        public override void Bind(Album item)
        {
            this.Title.Text = item.Name;
            this.Artist.Text = item.Artist.Name;
            this.ImageUrl = item.Image;
        }
    }
}