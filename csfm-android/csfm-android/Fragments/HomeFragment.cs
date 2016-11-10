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

        public override void OnStart()
        {
            base.OnStart();
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
                Artist = artist,
            };
            Track track2 = new Track
            {
                Album = album,
                Name = "A-YO",
                Artist = artist
            };


            historic.Add(new History(track1, DateTime.Now.AddHours(-2)));
            historic.Add(new History(track2, DateTime.Now));

            HistoryAdapter adapter = new HistoryAdapter(this.Activity, historic);

            recyclerView.SetAdapter(adapter);


            if (ScrobblePrefs.IsPlaying && ScrobblePrefs.HasValue)
            {
                Artist artistScrobble = new Artist
                {
                    Name = ScrobblePrefs.Artist
                };

                Album albumScrobble = new Album
                {
                    Name = ScrobblePrefs.Album,
                    Image = MusicLibrary.GetAlbumArt(ScrobblePrefs.Artist, ScrobblePrefs.Album, rootView.Context).FirstOrDefault()

                };

                Track trackScrobble = new Track
                {
                    Album = albumScrobble,
                    Artist = artistScrobble,
                    Name = ScrobblePrefs.Track
                };

                adapter.AddScrobble(new History(trackScrobble, true));

            }

            
        }
    }
}