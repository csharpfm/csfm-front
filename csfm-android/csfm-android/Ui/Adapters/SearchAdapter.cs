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
    public abstract class SearchAdapter : RecyclerView.Adapter
    {
        private List<History> data;
        private View noResult;
        private RecyclerView recyclerView;

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

        public override int ItemCount
        {
            get
            {
                return data != null && data.Count > 0 ? data.Count : 1;
            }
        }

        public SearchAdapter(Context context, List<History> data)
        {
            this.data = data;
            this.recyclerView = recyclerView;
        }

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
                (holder as HistoryHolder)?.BindNoResult();
            }
        }
    }
}