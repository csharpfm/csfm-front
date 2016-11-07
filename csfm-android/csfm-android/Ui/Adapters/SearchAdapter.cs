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
using csfm_android.Holders;

namespace csfm_android.Adapters
{
    public abstract class SearchAdapter<T> : RecyclerView.Adapter where T : MusicItem
    {
        private List<T> data;
        private Context context;

        public List<T> Data
        {
            set
            {
                data = value;
                NotifyDataSetChanged();
            }
        }

        public SearchAdapter(Context context, List<T> data)
        {
            this.data = data;
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return data != null ? data.Count : 0;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SearchHolder<T> searchHolder = holder as SearchHolder<T>;
            searchHolder?.Bind(data[position]);
        }
    }
}