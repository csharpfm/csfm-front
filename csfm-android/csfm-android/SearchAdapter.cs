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
    public class SearchAdapter<T> : RecyclerView.Adapter
    {
        private List<T> data;

        private Context context;

        public SearchAdapter(Context context, List<T> data)
        {
            this.data = data;
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return data.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SearchHolder searchHolder = holder as SearchHolder;
            searchHolder?.Bind(data[position] as string); //FIXME
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new SearchHolder(LayoutInflater.From(parent.Context).Inflate(SearchHolder.LAYOUT, parent, false));
        }
    }
}