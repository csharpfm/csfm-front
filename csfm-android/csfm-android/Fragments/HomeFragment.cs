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

        public override void OnStart()
        {
            base.OnStart();

            GetHistory();

         /*   List<History> historic = new List<History>();
            Album album = new Album();
            Artist artist = new Artist();
            artist.Name = "Lady Gaga";
            album.Artist = artist;
            album.Name = "Joanne";
            album.Image = "https://lh4.googleusercontent.com/-p1ejPKmyA2s/AAAAAAAAAAI/AAAAAAACRXU/6S-Em-MWl08/s0-c-k-no-ns/photo.jpg";
            Track track1 = new Track();
            track1.Album = album;
            track1.Name = "Perfect Illusion";
            Track track2 = new Track();
            track2.Album = album;
            track2.Name = "A-YO";


            historic.Add(new Api.Model.History(track1, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));
            historic.Add(new Api.Model.History(track2, new DateTime()));

            recyclerView.SetAdapter(new HistoryAdapter(this.Activity, historic));*/
        }

        private async void GetHistory()
        {
            var apiClient = new ApiClient();

            var history = await apiClient.GetHistory("Siliem");

            if(String.IsNullOrEmpty(history) || "[]".Equals(history))
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