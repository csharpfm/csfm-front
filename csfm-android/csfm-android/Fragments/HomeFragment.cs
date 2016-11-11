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
using csfm_android.Ui.Adapters;
using csfm_android.Api.Model;
using csfm_android.Utils;
using csfm_android.Api;
using Android.Locations;

namespace csfm_android.Fragments
{
    public class HomeFragment : Fragment
    {
       
        private View rootView;

        private RecyclerView recyclerView;

        private TextView noHistory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.rootView = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            recyclerView = this.rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            noHistory = this.rootView.FindViewById<TextView>(Resource.Id.no_history_text);

            LinearLayoutManager layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            return this.rootView;
        }

        public override void OnResume()
        {
            base.OnResume();
            GetHistory();
        }

        private async void GetHistory()
        {
            var apiClient = new ApiClient();

            var history = await apiClient.GetHistory(CSFMPrefs.Prefs.GetString(CSFMApplication.Username, ""));

            if(history == null)
            {
                // No history found
            }
            else
            {
                this.noHistory.Visibility = ViewStates.Gone;
                //recyclerView.SetAdapter(new HistoryAdapter(this.Activity, historic));
                // GOOD
            }
        }

        public void onResponseReceived(string response)
        {
            throw new NotImplementedException();
        }
    }
}