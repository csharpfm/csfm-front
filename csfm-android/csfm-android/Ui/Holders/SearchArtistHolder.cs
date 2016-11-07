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
    public class SearchArtistHolder : SearchHolder<Artist>
    {
        public const int LAYOUT = Resource.Layout.search_artist_item;
        public TextView Title { get; set; }
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

        public SearchArtistHolder(View itemView) : base(itemView)
        {
            this.Title = itemView.FindViewById<TextView>(Resource.Id.search_artist_name);
            this.Image = itemView.FindViewById<ImageView>(Resource.Id.search_image);
        }

        public override void Bind(Artist item)
        {
            this.Title.Text = item.Name;
            this.ImageUrl = item.Image;
        }
    }
}