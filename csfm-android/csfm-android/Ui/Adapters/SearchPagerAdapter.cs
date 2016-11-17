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
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using csfm_android.Api.Model;
using csfm_android.Fragments;
using csfm_android.Activities;

namespace csfm_android.Ui.Adapters
{
    public class SearchPagerAdapter : FragmentPagerAdapter
    {
        public string Query { get; set; }

        public List<Lazy<SearchFragment>> fragments;

        public SearchPagerAdapter(ToolbarActivity activity, string query) : base(activity.SupportFragmentManager)
        {
            this.Query = query;
            this.fragments = new List<Lazy<SearchFragment>>(3)
            {
                new Lazy<SearchFragment>(() => new SearchArtistFragment(this)),
                new Lazy<SearchFragment>(() => new SearchAlbumFragment(this)),
                new Lazy<SearchFragment>(() => new SearchTrackFragment(this))
            };
        }

        public Fragment this[int i]
        {
            get
            {
                return GetItem(i);
            }
        }

        public override int Count
        {
            get
            {
                return fragments != null ? fragments.Count : 0;
            }
        }



        public override Fragment GetItem(int position)
        {
            if (position >= 0 && position < fragments.Count)
            {
                return fragments[position].Value;
            }
            return null;
        }

        public void Update(string query)
        {
            this.Query = query;
            foreach(Lazy<SearchFragment> f in fragments)
            {
                if (f.IsValueCreated)
                {
                    f.Value.Update(query);
                }
            }
        }
    }
}