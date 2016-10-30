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
    public class SearchAdapter<T> : RecyclerView.Adapter where T : MusicItem
    {
        private List<Artist> fake = new List<Artist> { new Artist { Name = "Hello", Image = "https://f4.bcbits.com/img/a0648921701_16.jpg" }, new Artist { Name = "World", Image = "https://f4.bcbits.com/img/a0648921701_16.jpg" }, new Artist { Name = "Test", Image = "https://f4.bcbits.com/img/a0648921701_16.jpg" } };
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
                return fake.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //SearchHolder<T> searchHolder = holder as SearchHolder<T>;
            SearchHolder<Artist> searchHolder = holder as SearchHolder<Artist>;
            searchHolder?.Bind(fake[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (typeof(T) == typeof(Artist))
            {
                return new SearchArtistHolder(LayoutInflater.From(parent.Context).Inflate(SearchArtistHolder.LAYOUT, parent, false));
            }
            else if (typeof(T) == typeof(Album)) {
                //return new SearchHolder(LayoutInflater.From(parent.Context).Inflate(SearchHolder.LAYOUT, parent, false));
            }
            else if (typeof(T) == typeof(Track))
            {
                //return new SearchHolder(LayoutInflater.From(parent.Context).Inflate(SearchHolder.LAYOUT, parent, false));
            }
            return null;
        }
    }
}