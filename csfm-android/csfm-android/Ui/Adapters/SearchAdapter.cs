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
            SearchHolder<T> searchHolder = holder as SearchHolder<T>;
            searchHolder?.Bind(data[position]);
        }

        //public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        //{
        //    if (typeof(T) == typeof(Artist))
        //    {
        //        return new SearchArtistHolder(LayoutInflater.From(parent.Context).Inflate(SearchArtistHolder.LAYOUT, parent, false));
        //    }
        //    else if (typeof(T) == typeof(Album)) {
        //        //return new SearchHolder(LayoutInflater.From(parent.Context).Inflate(SearchHolder.LAYOUT, parent, false));
        //    }
        //    else if (typeof(T) == typeof(Track))
        //    {
        //        //return new SearchHolder(LayoutInflater.From(parent.Context).Inflate(SearchHolder.LAYOUT, parent, false));
        //    }
        //    return null;
        //}
    }
}