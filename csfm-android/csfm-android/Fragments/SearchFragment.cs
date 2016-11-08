using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using csfm_android.Api.Model;
using csfm_android.Ui.Adapters;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace csfm_android.Fragments
{
    public abstract class SearchFragment : Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        protected View rootView;
        protected RecyclerView recyclerView;
        protected SwipeRefreshLayout refresh;

        public static string FAKE_IMAGE = "https://f4.bcbits.com/img/a0648921701_16.jpg";
        public static List<Artist> FAKE_ARTISTS, FAKE_ARTISTS2;
        public static List<Album> FAKE_ALBUMS;
        public static List<Track> FAKE_TRACKS;

        public SearchPagerAdapter PagerAdapter { get; private set; }

        public SearchFragment(SearchPagerAdapter pagerAdapter)
        {
            this.PagerAdapter = pagerAdapter;
            initFakeList();
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
            this.refresh.SetColorSchemeResources(Resource.Color.colorPrimary);
            this.refresh.SetOnRefreshListener(this);
            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();
            SetRecyclerViewLayoutManager(recyclerView);
            SetRecyclerViewAdapter(recyclerView);
            this.Update(this.PagerAdapter.Query);
        }

        protected abstract void SetRecyclerViewLayoutManager(RecyclerView recyclerView);

        protected abstract void SetRecyclerViewAdapter(RecyclerView recyclerView);

        public void Update(string name)
        {
            refresh.Refreshing = true;
            this.Update(name, () => refresh.Refreshing = false);
        }
        protected abstract void Update(string name, Action callback);

        protected abstract void Update(Action callback);

        public void OnRefresh()
        {
            this.Update(() => refresh.Refreshing = false);
        }

        private void initFakeList()
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
                new Track { Name = "Track1", Album = FAKE_ALBUMS[0], Duration = 123 },
                new Track { Name = "Track2", Album = FAKE_ALBUMS[0], Duration = 250 },
                new Track { Name = "Track3", Album = FAKE_ALBUMS[0], Duration = 360 },
                new Track { Name = "Track4", Album = FAKE_ALBUMS[0], Duration = 98 },
                new Track { Name = "Track5", Album = FAKE_ALBUMS[1], Duration = 836 },
                new Track { Name = "Track6", Album = FAKE_ALBUMS[1], Duration = 543 }
            };
        }
    }
}