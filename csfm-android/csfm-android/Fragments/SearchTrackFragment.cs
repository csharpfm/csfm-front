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
    public class SearchTrackFragment : SearchFragment
    {
        private SearchTrackAdapter adapter;
        protected SearchTrackAdapter Adapter
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

        public SearchTrackFragment(SearchPagerAdapter pagerAdapter) : base(pagerAdapter)
        {
        }

        private void SetRecyclerViewAdapter(RecyclerView recyclerView, SearchTrackAdapter adapter)
        {
            recyclerView.SetAdapter(this.adapter = adapter);
        }

        protected override void SetRecyclerViewAdapter(RecyclerView recyclerView)
        {
            SetRecyclerViewAdapter(recyclerView, new SearchTrackAdapter(this.Context, new List<Track>()));
        }

        protected override void SetRecyclerViewLayoutManager(RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
        }

        protected override void Update(Action callback)
        {
            Adapter.Data = FAKE_TRACKS;
            callback();
        }

        protected override void Update(string name, Action callback)
        {
            Adapter.Data = FAKE_TRACKS;
            callback();
        }
    }
}