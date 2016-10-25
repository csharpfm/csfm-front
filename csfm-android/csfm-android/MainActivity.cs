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
    public class MainActivity : AppCompatActivity
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
            
            startHomeFragment();

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
                bottomBar.MapColorForTab(i, "#F44336");
            }
        }

        private void startHomeFragment()
        {
            // Create a new fragment and a transaction.
            FragmentTransaction fragmentTransaction = this.FragmentManager.BeginTransaction();
            HomeFragment homeFragment = new HomeFragment();

            // The fragment will have the ID of Resource.Id.fragment_container.
            fragmentTransaction.Add(Resource.Id.mainContainer, homeFragment);

            // Commit the transaction.
            fragmentTransaction.Commit();
        }
    }
}

