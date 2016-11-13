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

namespace csfm_android.Fragments
{
    public class HomeFragment : Fragment
    {
       
        private View rootView;

        private RecyclerView recyclerView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.rootView = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            recyclerView = this.rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
          
            LinearLayoutManager layoutManager = new LinearLayoutManager(this.Activity);
            recyclerView.SetLayoutManager(layoutManager);

            return this.rootView;
        }

        public override void OnResume()
        {
            base.OnResume();

            List<History> historic = new List<History>();

            Artist artist = new Artist()
            {
                Name = "Lady Gaga"
            };
            Album album = new Album()
            {
                Artist = artist,
                Name = "Joanne",
                Image = "https://lh4.googleusercontent.com/-p1ejPKmyA2s/AAAAAAAAAAI/AAAAAAACRXU/6S-Em-MWl08/s0-c-k-no-ns/photo.jpg"
            };
            Track track1 = new Track
            {
                Album = album,
                Name = "Perfect Illusion",
            };
            Track track2 = new Track
            {
                Album = album,
                Name = "A-YO",
            };


            historic.Add(new History(track1, DateTime.Now.AddHours(-2)));
            historic.Add(new History(track2, DateTime.Now));

            HistoryAdapter adapter = new HistoryAdapter(this.Activity, historic);
            recyclerView.SetAdapter(adapter);

            GetHistory();
        }

        private async void GetHistory()
        {
            var apiClient = new ApiClient();

            var history = await apiClient.GetHistory(CSFMPrefs.Prefs.GetString(CSFMApplication.Username, ""));

            if(history != null)
            {
                HistoryAdapter adapter = new HistoryAdapter(this.Activity, history);
                InitScrobble(adapter, history);
                recyclerView.SetAdapter(adapter);
                // GOOD
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
    }
}