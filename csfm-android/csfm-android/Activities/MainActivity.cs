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
using Android.Support.V7.Widget;
using csfm_android.Services;

namespace csfm_android.Activities
{

    [Activity(Label = Configuration.LABEL, Icon = "@drawable/icon", Theme = "@style/MyTheme", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : ToolbarActivity, BottomNavigationBar.Listeners.IOnMenuTabClickListener
    {
        private BottomBar bottomBar;
        private Fragment currentFragment = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.Main, GetString(Resource.String.home));
            SetBottomBar(bundle);
        }

        protected override void OnResume()
        {
            base.OnResume();
            MaterialSearchView.CloseSearch();
            ScrobblerService.InitService(this.ApplicationContext);
        }

        public override void OnSearchViewSet()
        {
            //
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
            Console.WriteLine("Hello world " + menuItemId);
        }

        private void SetBottomBar(Bundle bundle)
        {
            bottomBar = BottomBar.Attach(this, bundle);

            BottomBarTab[] tabs = new BottomBarTab[3]
            {
                 new BottomBarTab(Resource.Drawable.ic_home_white_24dp, Resource.String.home),
                 new BottomBarTab(Resource.Drawable.ic_favorite_white_24dp, Resource.String.match),
                 new BottomBarTab(Resource.Drawable.ic_person_white_24dp, Resource.String.account)
            };

            bottomBar.SetFixedInactiveIconColor("#44000000");
            bottomBar.SetActiveTabColor("#F44336");

            bottomBar.SetItems(tabs);
            for (int i = 0; i < tabs.Length; i++)
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
                    this.Toolbar.Title = GetString(Resource.String.home);
                    break;
                case 1:
                    LaunchFragment(new MatchFragment());
                    this.Toolbar.Title = GetString(Resource.String.match);
                    break;
                case 2:
                    LaunchFragment(new AccountFragment());
                    this.Toolbar.Title = GetString(Resource.String.account);
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

