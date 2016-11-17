using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using csfm_android.Api.Model;
using csfm_android.Ui.Adapters;
using System.Linq;
using System;
using System.Linq.Expressions;
using Android.Widget;

namespace csfm_android.Fragments
{
    public abstract class SearchFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        protected View rootView;
        protected RecyclerView recyclerView;
        protected SwipeRefreshLayout refresh;
        protected TextView noResult;

        public SearchPagerAdapter PagerAdapter { get; private set; }

        public SearchFragment(SearchPagerAdapter pagerAdapter)
        {
            this.PagerAdapter = pagerAdapter;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            rootView = inflater.Inflate(Resource.Layout.search_activity_tab, container, false);
            this.noResult = rootView.FindViewById<TextView>(Resource.Id.searchNoResult);
            this.recyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.searchRecyclerView);
            this.refresh = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh);
            this.refresh.SetColorSchemeResources(Resource.Color.colorPrimary);
            this.refresh.SetOnRefreshListener(this);
            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();
            SetRecyclerViewLayoutManager(recyclerView);
            SetRecyclerViewAdapter(recyclerView, noResult);
            this.Update(this.PagerAdapter.Query);
        }

        protected abstract void SetRecyclerViewLayoutManager(RecyclerView recyclerView);

        protected abstract void SetRecyclerViewAdapter(RecyclerView recyclerView, View noResult);

        public void Update(string name)
        {
            refresh.Refreshing = true;
            this.Update(name, () => refresh.Refreshing = false);
        }
        protected abstract void Update(string name, Action callback);

        protected abstract void Update(Action callback);

        public void OnRefresh()
        {
            this.Update(() => refresh.Refreshing = false);
        }

        
    }
}