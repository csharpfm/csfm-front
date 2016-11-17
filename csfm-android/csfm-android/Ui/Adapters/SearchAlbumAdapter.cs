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
using Android.Support.V7.Widget;
using csfm_android.Ui.Holders;

namespace csfm_android.Ui.Adapters
{
    /// <summary>
    /// SearchAlbumFragment RecyclerView's adapter
    /// </summary>
    public class SearchAlbumAdapter : SearchAdapter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        public SearchAlbumAdapter(Context context, List<History> data) : base(context, data)
        {
        }

        /// <summary>
        /// On item view creation
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new HistoryHolder(LayoutInflater.From(parent.Context).Inflate(HistoryHolder.LAYOUT, parent, false));
        }
    }
}