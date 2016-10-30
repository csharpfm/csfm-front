using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using csfm_android.Api.Model;

namespace csfm_android
{
    public class SearchFragment<T> : Fragment, SwipeRefreshLayout.IOnRefreshListener where T : MusicItem
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

            if (typeof(T) == typeof(Track))
            {
                recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
            }
            else
            {
                recyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 3));
            }
            //recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
            //SetAdapter(new List<T>() { "hello", "world", "test" });
            SetAdapter(new List<T>());
        }

        private void SetAdapter(List<T> data)
        {
            recyclerView.SetAdapter(new SearchAdapter<T>(this.Context, data));
        }

        public void OnRefresh()
        {
            
        }
    }
}