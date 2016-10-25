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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "MatchFM";

            setBottomBar(bundle);
            
            StartHomeFragment();

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

            BottomBarTab[] tabs = new BottomBarTab[5];
            tabs[0] = new BottomBarTab(Resource.Drawable.ic_home_white_24dp, "Home");
            tabs[1] = new BottomBarTab(Resource.Drawable.ic_menu_search_holo_dark, "Search");
            tabs[2] = new BottomBarTab(Resource.Drawable.ic_favorite_white_24dp, "Match");
            tabs[3] = new BottomBarTab(Resource.Drawable.ic_chat_white_24dp, "Chat");
            tabs[4] = new BottomBarTab(Resource.Drawable.ic_account_box_white_24dp, "Account");

            bottomBar.SetItems(tabs);
            for (int i = 0; i < 5; i++)
            {
                tabs[i].Id = i;
                bottomBar.MapColorForTab(i, "#F44336");
            }

            bottomBar.SetOnMenuTabClickListener(this);
           
        }

        private void StartHomeFragment()
        {
            // Create a new fragment and a transaction.
            FragmentTransaction fragmentTransaction = this.FragmentManager.BeginTransaction();
            HomeFragment homeFragment = new HomeFragment();

            fragmentTransaction.Add(Resource.Id.mainContainer, homeFragment);
            fragmentTransaction.Commit();
        }

        private void LaunchFragment(Fragment fragment)
        {
            FragmentTransaction fragmentTransaction = this.FragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.mainContainer, fragment);
            fragmentTransaction.Commit();
        }

        public void OnMenuTabSelected(int menuItemId)
        {
            switch(menuItemId)
            {
                case 0:
                    LaunchFragment(new HomeFragment());
                    break;
                case 1:
                    // search
                    break;
                case 2:
                    //LaunchFragment(new MatchFragment());
                    break;
                case 3:
                    // chat
                    break;
                case 4:
                    // user account
                    break;
                default:
                    break;
            }
        }

        public void OnMenuTabReSelected(int menuItemId)
        {
            Console.WriteLine("RE " + menuItemId);
        }
    }
}

