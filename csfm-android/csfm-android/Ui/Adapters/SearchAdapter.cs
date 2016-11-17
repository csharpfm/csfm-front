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
using csfm_android.Ui.Holders;

namespace csfm_android.Ui.Adapters
{
    /// <summary>
    /// Abstract class used as SearchFragment RecyclerView's adapter (used by SearchArtistAdapter, SearchAlbumAdapter and SearchTrackAdapter)
    /// </summary>
    public abstract class SearchAdapter : RecyclerView.Adapter
    {
        private List<History> data;

        /// <summary>
        /// Updates the view
        /// </summary>
        public List<History> Data
        {
            set
            {
                this.data = value;
                NotifyDataSetChanged();
            }

            get
            {
                return data;
            }
        }


        /// <summary>
        /// Number of items. In case of no items, will return 1 as to render a dummy element with "No results" label.
        /// </summary>
        public override int ItemCount
        {
            get
            {
                return data != null && data.Count > 0 ? data.Count : 1;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        public SearchAdapter(Context context, List<History> data)
        {
            this.data = data;
        }


        /// <summary>
        /// On item bind to a view
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //SearchHolder<T> searchHolder = holder as SearchHolder<T>;
            //searchHolder?.Bind(Data[position]);
            if (Data.Count > position)
            {
                (holder as HistoryHolder)?.Bind(Data[position]);
            }
            else
            {
                (holder as HistoryHolder)?.BindNoResult(); //In case of no result, add a dummy element with "No result" label
            }
        }
    }
}