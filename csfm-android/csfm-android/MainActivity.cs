using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using BottomNavigationBar;
using Android.Support.V7.App;
using Android.Support.V4.Content;
using csfm_android.Fragments;

namespace csfm_android
{
    [Activity(Label = "csfm_android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, BottomNavigationBar.Listeners.IOnMenuTabClickListener
    {
        private Toolbar toolbar;

        private BottomBar bottomBar;

        private Fragment currentFragment = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Home";

            setBottomBar(bundle);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public void OnMenuItemSelected(int menuItemId)
        {
            throw new NotImplementedException();
        }

        private void setBottomBar(Bundle bundle)
        {
            bottomBar = BottomBar.Attach(this, bundle);

            BottomBarTab[] tabs = new BottomBarTab[3];
            tabs[0] = new BottomBarTab(Resource.Drawable.ic_home_white_24dp, "Home");
            tabs[1] = new BottomBarTab(Resource.Drawable.ic_favorite_white_24dp, "Match");
            tabs[2] = new BottomBarTab(Resource.Drawable.ic_person_white_24dp, "Account");

            bottomBar.SetFixedInactiveIconColor("#44000000");
            bottomBar.SetActiveTabColor("#F44336");

            bottomBar.SetItems(tabs);
            for (int i = 0; i < 3; i++)
            {
                tabs[i].Id = i;
                bottomBar.MapColorForTab(i, "#EFEFEF");
            }

            bottomBar.SetOnMenuTabClickListener(this);
        }


        private void StartHomeFragment()
        {
            // Create a new fragment and a transaction.
            FragmentTransaction fragmentTransaction = this.FragmentManager.BeginTransaction();
            HomeFragment homeFragment = new HomeFragment();

            fragmentTransaction.Add(Resource.Id.mainContainer, homeFragment);
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }

        private void LaunchFragment(Fragment fragment)
        {
            FragmentTransaction fragmentTransaction = this.FragmentManager.BeginTransaction();
            if (currentFragment != null)
            {
                fragmentTransaction.Remove(currentFragment);
            }

            currentFragment = fragment;

            fragmentTransaction.Add(Resource.Id.mainContainer, fragment);
            fragmentTransaction.Commit();
        }

        public void OnMenuTabSelected(int menuItemId)
        {
            switch(menuItemId)
            {
                case 0:
                    LaunchFragment(new HomeFragment());
                    ActionBar.Title = "Home";
                    break;
                case 1:
                    LaunchFragment(new MatchFragment());
                    ActionBar.Title = "Match";
                    break;
                case 2:
                    ActionBar.Title = "Account";
                    // chat
                    break;
                default:
                    break;
            }
        }

        public void OnMenuTabReSelected(int menuItemId)
        {
          
        }
    }
}

