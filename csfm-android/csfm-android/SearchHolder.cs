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
using Android.Support.V7.Widget;

namespace csfm_android
{
    public class SearchHolder : RecyclerView.ViewHolder
    {
        public const int LAYOUT = Resource.Layout.search_item;
        public TextView Title { get; set; }

        public SearchHolder(View itemView) : base(itemView)
        {
            this.Title = itemView.FindViewById<TextView>(Resource.Id.search_title);
        }

        public void Bind(string title)
        {
            this.Title.Text = title;
        }
    }
}