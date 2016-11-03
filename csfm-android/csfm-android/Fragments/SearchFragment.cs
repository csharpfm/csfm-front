using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using csfm_android.Api.Model;
using csfm_android.Adapters;
using System.Linq;

namespace csfm_android.Fragments
{
    public abstract class SearchFragment<T> : Fragment, SwipeRefreshLayout.IOnRefreshListener where T : MusicItem
    {
        private View rootView;
        private RecyclerView recyclerView;
        private SwipeRefreshLayout refresh;

        public static string FAKE_IMAGE = "https://f4.bcbits.com/img/a0648921701_16.jpg";
        public static List<Artist> FAKE_ARTISTS;
        public static List<Album> FAKE_ALBUMS;
        public static List<Track> FAKE_TRACKS;

        public SearchFragment()
        {
            if (FAKE_ARTISTS != null) return;

            FAKE_ARTISTS = new List<Artist> {
                new Artist { Name = "Hello", Image =  FAKE_IMAGE },
                new Artist { Name = "World", Image = FAKE_IMAGE },
                new Artist { Name = "Test", Image = FAKE_IMAGE }
            };

            FAKE_ALBUMS = new List<Album>
            {
                new Album { Name = "Album1", Image = FAKE_IMAGE, Artist = FAKE_ARTISTS[0] },
                new Album { Name = "Album2", Image = FAKE_IMAGE, Artist = FAKE_ARTISTS[0] },
                new Album { Name = "Album3", Image = FAKE_IMAGE, Artist = FAKE_ARTISTS[0] },
                new Album { Name = "Album4", Image = FAKE_IMAGE, Artist = FAKE_ARTISTS[1] },
                new Album { Name = "Album6", Image = FAKE_IMAGE, Artist = FAKE_ARTISTS[1] },
                new Album { Name = "Album7", Image = FAKE_IMAGE, Artist = FAKE_ARTISTS[2] }
            };

            for (int i = 0; i < FAKE_ARTISTS.Count; i++)
            {
                FAKE_ARTISTS[i].Albums = FAKE_ALBUMS.Where(a => a.Artist.Name == FAKE_ARTISTS[i].Name).ToList();
            }

            FAKE_TRACKS = new List<Track>
            {
                new Track { Name = "Track1", Album = FAKE_ALBUMS[0], Artist = FAKE_ALBUMS[0].Artist },
                new Track { Name = "Track2", Album = FAKE_ALBUMS[0], Artist = FAKE_ALBUMS[0].Artist },
                new Track { Name = "Track3", Album = FAKE_ALBUMS[0], Artist = FAKE_ALBUMS[0].Artist },
                new Track { Name = "Track4", Album = FAKE_ALBUMS[0], Artist = FAKE_ALBUMS[0].Artist },
                new Track { Name = "Track5", Album = FAKE_ALBUMS[1], Artist = FAKE_ALBUMS[0].Artist },
                new Track { Name = "Track6", Album = FAKE_ALBUMS[1], Artist = FAKE_ALBUMS[0].Artist }
            };
        }

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
            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();

            //if (typeof(T) == typeof(Track))
            //{
            //    recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
            //}
            //else
            //{
            //    recyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 3));
            //}
            SetRecyclerViewLayoutManager(recyclerView);
            SetRecyclerViewAdapter(recyclerView);
        }

        protected abstract void SetRecyclerViewLayoutManager(RecyclerView recyclerView);

        protected abstract void SetRecyclerViewAdapter(RecyclerView recyclerView);

        public void OnRefresh()
        {
            
        }
    }
}