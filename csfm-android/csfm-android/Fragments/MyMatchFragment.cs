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
using Android.Support.V7.Widget;
using csfm_android.Api;
using csfm_android.Utils;
using csfm_android.Ui.Adapters;

namespace csfm_android.Fragments
{
    /// <summary>
    /// MyMatch Fragment : View your matched users
    /// </summary>
    public class MyMatchFragment : Fragment
    {

        private View rootView;

        private RecyclerView recyclerView;

        private MyMatchAdapter adapter;

        /// <summary>
        /// On fragment creation
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
            this.rootView = inflater.Inflate(Resource.Layout.my_match_fragment, container, false);

            recyclerView = this.rootView.FindViewById<RecyclerView>(Resource.Id.my_match_recycler);

            LinearLayoutManager layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            return this.rootView;
        }

        /// <summary>
        /// On fragment resume
        /// </summary>
        public override void OnResume()
        {
            base.OnResume();
            GetMatch();
        }

        /// <summary>
        /// Get list of matched users and displays it on the fragment
        /// </summary>
        private async void GetMatch()
        {
            var users = await new ApiClient().GetUserMatch(CSFMPrefs.Prefs.GetString(CSFMApplication.Username, ""));

            this.adapter = new MyMatchAdapter(this.Activity, users);
            recyclerView.SetAdapter(this.adapter);
        }
    }
}