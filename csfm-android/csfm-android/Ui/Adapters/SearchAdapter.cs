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
    public abstract class SearchAdapter<T> : AbstractAdapter<T> where T : MusicItem
    {
        public SearchAdapter(Context context, List<T> data) : base(context, data)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SearchHolder<T> searchHolder = holder as SearchHolder<T>;
            searchHolder?.Bind(Data[position]);
        }
    }
}