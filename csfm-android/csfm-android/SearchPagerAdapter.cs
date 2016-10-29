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

namespace csfm_android
{
    public class SearchPagerAdapter : FragmentPagerAdapter
    {

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
                    Console.WriteLine("Artists");
                    break;
                case 1:
                    Console.WriteLine("Album");
                    break;
                case 2:
                    Console.WriteLine("Songs");
                    break;
                default:
                    break;
            }

            return new SearchFragment();

        }
    }
}