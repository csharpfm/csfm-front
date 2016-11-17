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

namespace csfm_android.Ui.Holders
{
    /// <summary>
    /// Abstract search holder used by SearchAlbumHolder, SearchArtistHolder and SearchTrackHolder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SearchHolder<T> : RecyclerView.ViewHolder
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="itemView"></param>
        public SearchHolder(View itemView) : base(itemView)
        {
        }

        /// <summary>
        /// Bind a new item
        /// </summary>
        /// <param name="item"></param>
        public abstract void Bind(T item);
    }
}