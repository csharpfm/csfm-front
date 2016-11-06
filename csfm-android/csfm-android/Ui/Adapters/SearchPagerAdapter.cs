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

namespace csfm_android.Adapters
{
    public class SearchPagerAdapter : FragmentPagerAdapter
    {
        public List<Fragment> fragments = new List<Fragment>(3) { null, null, null };

        public SearchPagerAdapter(ToolbarActivity activity) : base(activity.SupportFragmentManager)
        {

        }

        public override int Count
        {
            get
            {
                return 3;
            }
        }

        public override Fragment GetItem(int position)
        {
            switch(position)
            {
                case 0: //Artists
                    return fragments[position] == null ? (fragments[position] = new SearchArtistFragment()) : fragments[position];
                case 1: //Albums
                    return fragments[position] == null ? (fragments[position] = new SearchAlbumFragment()) : fragments[position];
                case 2: //Tracks
                    return fragments[position] == null ? (fragments[position] = new SearchTrackFragment()) : fragments[position];
                default:
                    return null;
            }


        }
    }
}