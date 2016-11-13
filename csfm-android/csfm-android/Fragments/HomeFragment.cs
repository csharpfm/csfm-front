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

using Android.Media;
using static Android.Media.MediaPlayer;
using Android.Provider;
using Android.Support.V4.Widget;
using csfm_android.Activities;
using csfm_android.Utils.MaterialDesignSearchView;

namespace csfm_android.Fragments
{
    public class HomeFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        private View rootView;

        private RecyclerView recyclerView;

        private SwipeRefreshLayout refresh;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.rootView = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            this.recyclerView = this.rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            this.refresh = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.home_swipe_refresh);
            this.refresh.SetColorSchemeResources(Resource.Color.colorPrimary);
            this.refresh.SetOnRefreshListener(this);

            LinearLayoutManager layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            return this.rootView;
        }

        public override void OnResume()
        {
            base.OnResume();

            Console.WriteLine(this.recyclerView.GetAdapter());

            refresh.Post(() => refresh.Refreshing = true);
            OnRefresh();
        }

        private async void GetHistory(Action<List<History>> callback)
        {
            var apiClient = new ApiClient();

            var history = await apiClient.GetHistory(CSFMPrefs.Prefs.GetString(CSFMApplication.Username, ""));

            if(history != null)
            {
                try
                {
                    HistoryAdapter adapter = new HistoryAdapter(this.Activity, history);
                    InitScrobble(adapter, history);
                    recyclerView.SetAdapter(adapter);
                    callback?.Invoke(history);
                }
                catch
                {
                    //Exception in case of fragment change
                }
                
            }
        }

        private void InitScrobble(HistoryAdapter adapter, List<History> history = null)
        {
            if (ScrobblePrefs.IsPlaying && ScrobblePrefs.HasValue && !ScrobblePrefs.IsSongEnded)
            {
                History firstItem = history?.FirstOrDefault();


                Track trackScrobble = new Track
                {
                    Album = new Api.Model.Album
                    {
                        Name = ScrobblePrefs.Album,
                        Image = MusicLibrary.GetAlbumArt(ScrobblePrefs.Artist, ScrobblePrefs.Album, rootView.Context).FirstOrDefault(),
                        Artist = new Artist { Name = ScrobblePrefs.Artist }
                    },
                    Name = ScrobblePrefs.Track
                };

                if (firstItem != null && firstItem.Track?.Name == trackScrobble.Name && firstItem.Track?.Album?.Name == trackScrobble.Album?.Name && firstItem.Track?.Album?.Artist?.Name == firstItem.Track?.Album?.Artist?.Name)
                {
                    firstItem.IsScrobbling = true;
                }
                else
                {
                    adapter.Scrobble = new History(trackScrobble, true);
                }


            }
            else
            {
                adapter.Scrobble = null;
            }

        }

        public void OnRefresh()
        {
            GetHistory(h => {
                refresh.Refreshing = false;
                try
                {
                    MaterialSearchView.SetSuggestions(h);
                    (Activity as ToolbarActivity).MaterialSearchView.Suggestions = csfm_android.Utils.MaterialDesignSearchView.SearchAdapter.SUGGESTIONS;
                }
                catch
                {

                }
            });
        }
    }
}