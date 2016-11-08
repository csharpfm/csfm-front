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

        public List<SearchFragment> fragments = new List<SearchFragment>(3) { null, null, null };

        public SearchPagerAdapter(ToolbarActivity activity, string query) : base(activity.SupportFragmentManager)
        {
            this.Query = query;
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
            switch(position)
            {
                case 0: //Artists
                    return fragments[position] == null ? (fragments[position] = new SearchArtistFragment(this)) : fragments[position];
                case 1: //Albums
                    return fragments[position] == null ? (fragments[position] = new SearchAlbumFragment(this)) : fragments[position];
                case 2: //Tracks
                    return fragments[position] == null ? (fragments[position] = new SearchTrackFragment(this)) : fragments[position];
                default:
                    return null;
            }


        }

        public void Update(string query)
        {
            this.Query = query;
            foreach(SearchFragment f in fragments)
            {
                f?.Update(query);
            }
        }
    }
}