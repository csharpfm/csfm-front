using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;

namespace csfm_android
{
    public class SearchFragment<T> : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        private View rootView;
        private RecyclerView recyclerView;
        private SwipeRefreshLayout refresh;

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

            this.recyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.searchRecyclerView);
            this.refresh = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh);
            this.refresh.SetOnRefreshListener(this);
            //return base.OnCreateView(inflater, container, savedInstanceState);
            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();

            recyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 2));
            //recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
            SetAdapter(new List<string>() { "hello", "world", "test" });
        }

        private void SetAdapter(List<string> data)
        {
            recyclerView.SetAdapter(new SearchAdapter<string>(this.Context, data));
        }

        public void OnRefresh()
        {
            
        }
    }
}