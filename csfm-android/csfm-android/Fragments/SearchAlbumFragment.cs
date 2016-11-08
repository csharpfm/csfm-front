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
using csfm_android.Ui.Adapters;

namespace csfm_android.Fragments
{
    public class SearchAlbumFragment : SearchFragment
    {
        private SearchAlbumAdapter adapter;
        protected SearchAlbumAdapter Adapter
        {
            get
            {
                if (adapter == null && recyclerView != null)
                {
                    SetRecyclerViewAdapter(recyclerView);
                }
                return adapter;
            }

            set
            {
                SetRecyclerViewAdapter(recyclerView, value);
            }
        }

        public SearchAlbumFragment(SearchPagerAdapter pagerAdapter) : base(pagerAdapter)
        {
        }

        private void SetRecyclerViewAdapter(RecyclerView recyclerView, SearchAlbumAdapter adapter)
        {
            recyclerView.SetAdapter(this.adapter = adapter);
        }

        protected override void SetRecyclerViewAdapter(RecyclerView recyclerView)
        {
            SetRecyclerViewAdapter(recyclerView, new SearchAlbumAdapter(this.Context, new List<Album>()));
        }

        protected override void SetRecyclerViewLayoutManager(RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 3));
        }

        protected override void Update(Action callback)
        {

            Adapter.Data = FAKE_ALBUMS;
            callback();
        }

        protected override void Update(string name, Action callback)
        {
            //data API Callback
            Adapter.Data = FAKE_ALBUMS;
            callback();
        }
    }
}