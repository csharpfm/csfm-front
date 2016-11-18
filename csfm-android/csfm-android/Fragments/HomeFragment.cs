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
    /// <summary>
    /// Home Fragment : View your history/scrobbles
    /// </summary>
    public class HomeFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        private View rootView;

        private RecyclerView recyclerView;

        private SwipeRefreshLayout refresh;

        private HistoryAdapter adapter;

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
            this.rootView = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            this.recyclerView = this.rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            this.refresh = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.home_swipe_refresh);
            this.refresh.SetColorSchemeResources(Resource.Color.colorPrimary);
            this.refresh.SetOnRefreshListener(this);

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

            if (this.adapter == null)
            {
                refresh.Post(() => refresh.Refreshing = true);
                OnRefresh();
            }
            else
            {
                recyclerView.SetAdapter(adapter);
            }  
        }

        /// <summary>
        /// Displays the history after retrieving it via an API request
        /// </summary>
        /// <param name="callback"></param>
        private async void GetHistory(Action<List<History>> callback)
        {
            ApiClient apiClient = new ApiClient();

            List<History> history = await apiClient.GetHistory(CSFMPrefs.Username);

            if (history == null) history = new List<History>(); //In case of an error

            if(history != null)
            {
                try
                {
                    this.adapter = new HistoryAdapter(this.Activity, history);
                    InitScrobble(adapter, history);
                    recyclerView.SetAdapter(adapter);
                    callback?.Invoke(history);
                }
                catch
                {
                    //Avoid exception in case of fragment change
                }
                
            }
        }

        /// <summary>
        /// Checks if a song is being played on the mobile device and displays the rotating history item if so.
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="history"></param>
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

                if (firstItem != null && firstItem.IsSameTrack(trackScrobble))
                {
                    //First item is the scrobble
                    firstItem.IsScrobbling = true;
                }
                else
                {
                    //Add a new song to the list
                    adapter.Scrobble = new History(trackScrobble, true);
                }
            }
            else
            {
                adapter.Scrobble = null;
            }
        }

        /// <summary>
        /// SwipeRefreshLayout is triggered : Refreshes the page
        /// </summary>
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