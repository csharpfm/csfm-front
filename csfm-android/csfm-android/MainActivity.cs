using Android.App;
using Android.OS;
using Android.Views;
using System;
using BottomNavigationBar;
using csfm_android.Fragments;
using Android.Runtime;
using Android.Content;
using static Android.Support.V7.Widget.SearchView;
using Android.Support.V4.View;

namespace csfm_android
{
    [Activity(Label = Configuration.LABEL, MainLauncher = true, Icon = "@drawable/icon", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : ToolbarActivity, BottomNavigationBar.Listeners.IOnMenuTabClickListener
    {
        private BottomBar bottomBar;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.Main);

            setBottomBar(bundle);
            
            StartHomeFragment();

        }

        protected override void OnResume()
        {
            base.OnResume();
            if (SearchView != null)
            {
                SearchView.SetQuery("", false);
                SearchView.Iconified = true;
            }
        }

        public override void OnSearchViewSet()
        {
            if (SearchView != null)
                SearchView.Iconified = true;
        }

        public override bool OnQueryTextChange(string newText)
        {
            return true;
        }

        public override bool OnQueryTextSubmit(string query)
        {
            Intent intent = new Intent(this, typeof(SearchActivity));
            intent.PutExtra(SearchActivity.EXTRA_MESSAGE, query);
            this.StartActivity(intent);
            return true;
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

