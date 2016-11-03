using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using csfm_android.Api.Model;
using csfm_android.Holders;

namespace csfm_android.Adapters
{
    public class SearchTrackAdapter : SearchAdapter<Track>
    {
        public SearchTrackAdapter(Context context, List<Track> data) : base(context, data)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new SearchTrackHolder(LayoutInflater.From(parent.Context).Inflate(SearchTrackHolder.LAYOUT, parent, false));
        }
    }
}