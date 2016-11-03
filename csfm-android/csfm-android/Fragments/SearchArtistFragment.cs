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
using csfm_android.Adapters;

namespace csfm_android.Fragments
{
    public class SearchArtistFragment : SearchFragment<Artist>
    {

        protected override void SetRecyclerViewLayoutManager(RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 3));
        }

        protected override void SetRecyclerViewAdapter(RecyclerView recyclerView)
        {
            recyclerView.SetAdapter(new SearchArtistAdapter(this.Context, FAKE_ARTISTS));
        }
    }
}