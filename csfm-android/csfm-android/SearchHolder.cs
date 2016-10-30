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
using csfm_android.Api.Model;

namespace csfm_android
{
    public abstract class SearchHolder<T> : RecyclerView.ViewHolder where T : MusicItem
    {
        public SearchHolder(View itemView) : base(itemView)
        {
        }

        public abstract void Bind(T item);
    }
}