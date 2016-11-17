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
    /// <summary>
    /// Abstract fragment used by SearchArtistFragment, SearchAlbumFragment and SearchTrackFragment
    /// </summary>
    public abstract class SearchFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        protected View rootView;
        protected RecyclerView recyclerView;
        protected SwipeRefreshLayout refresh;
        protected TextView noResult;

        /// <summary>
        /// Link to the tabs adapter
        /// </summary>
        public SearchPagerAdapter PagerAdapter { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pagerAdapter"></param>
        public SearchFragment(SearchPagerAdapter pagerAdapter)
        {
            this.PagerAdapter = pagerAdapter;
        }

        /// <summary>
        /// On fragment creation
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        /// <summary>
        /// On fragment view creation
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
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

        /// <summary>
        /// On fragment resume
        /// </summary>
        public override void OnResume()
        {
            base.OnResume();
            SetRecyclerViewLayoutManager(recyclerView);
            SetRecyclerViewAdapter(recyclerView, noResult);
            this.Update(this.PagerAdapter.Query);
        }

        /// <summary>
        /// Initializes the recycler view layout manager
        /// </summary>
        /// <param name="recyclerView"></param>
        protected abstract void SetRecyclerViewLayoutManager(RecyclerView recyclerView);

        /// <summary>
        /// Initializes the recycler view with an empty adapter
        /// </summary>
        /// <param name="recyclerView"></param>
        /// <param name="noResult"></param>
        protected abstract void SetRecyclerViewAdapter(RecyclerView recyclerView, View noResult);

        /// <summary>
        /// Searches for the new query in the history list
        /// </summary>
        /// <param name="name"></param>
        public void Update(string name)
        {
            refresh.Refreshing = true;
            this.Update(name, () => refresh.Refreshing = false);
        }

        /// <summary>
        /// Abstract function to implement : Searches for the new query in the history list and calls the callback (to end the loading animation)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        protected abstract void Update(string name, Action callback);

        /// <summary>
        /// Abstract function to implement : Updates the results and calls the callback (to end the loading animation)
        /// </summary>
        /// <param name="callback"></param>
        protected abstract void Update(Action callback);

        /// <summary>
        /// Update the results
        /// </summary>
        public void OnRefresh()
        {
            this.Update(() => refresh.Refreshing = false);
        }

        
    }
}