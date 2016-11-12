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
using csfm_android.Utils.MaterialDesignSearchView;

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
            SetRecyclerViewAdapter(recyclerView, new SearchTrackAdapter(this.Context, new List<History>()));
        }

        protected override void SetRecyclerViewLayoutManager(RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
        }

        protected override void Update(Action callback)
        {
            Adapter.Data = MaterialSearchView.History; //FAKE_TRACKS;
            callback();
        }

        protected override void Update(string name, Action callback)
        {
            string[] words = name.ToLower().Trim().Split(' ');
            var history = MaterialSearchView.History;
            var tracks = history.Where(h => words.All(w => h.Track.Name.Trim().ToLower().Contains(w)));
            Adapter.Data = tracks.ToList();//FAKE_TRACKS;
            callback();
        }
    }
}