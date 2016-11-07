using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using csfm_android.Api.Model;
using csfm_android.Adapters;

namespace csfm_android.Fragments
{
    public class SearchArtistFragment : SearchFragment
    {
        private static List<Artist> FAKE_ARTISTS2;
        private static string FAKE_IMAGE2 = "http://cdn.ubeez.com/wp-content/uploads/sites/17/2016/10/Loiseau-libert%C3%A9.jpg";

        private SearchArtistAdapter adapter;
        protected SearchArtistAdapter Adapter
        {
            get
            {
                if (adapter == null && recyclerView != null)
                {
                    SetRecyclerViewAdapter(recyclerView);
                }
                return adapter;
            }

            set
            {
                SetRecyclerViewAdapter(recyclerView, value);
            }
        }

        public SearchArtistFragment(SearchPagerAdapter pagerAdapter) : base(pagerAdapter)
        {
            if (FAKE_ARTISTS2 == null)
                FAKE_ARTISTS2 = new List<Artist> {
                    new Artist { Name = "Damien Saez", Image =  FAKE_IMAGE2 },
                    new Artist { Name = "Third", Image = FAKE_IMAGE2 },
                    new Artist { Name = "Test2", Image = FAKE_IMAGE2 }
                };

        }

        protected override void SetRecyclerViewLayoutManager(RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 3));
        }

        private void SetRecyclerViewAdapter(RecyclerView recyclerView, SearchArtistAdapter adapter)
        {
            recyclerView.SetAdapter(this.adapter = adapter);
        }

        protected override void SetRecyclerViewAdapter(RecyclerView recyclerView)
        {
            SetRecyclerViewAdapter(recyclerView, new SearchArtistAdapter(this.Context, new List<Artist>()));
        }


        protected override void Update(Action callback)
        {
            //API Call
            Adapter.Data = FAKE_ARTISTS;
            callback();
        }

        protected override void Update(string name, Action callback)
        {
            //API Call
            Adapter.Data = FAKE_ARTISTS;
            callback();
        }
    }
}