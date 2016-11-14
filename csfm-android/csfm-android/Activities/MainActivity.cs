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
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using csfm_android.Api;
using csfm_android.Utils;

using csfm_android.Services;

namespace csfm_android.Activities
{

    [Activity(Label = Configuration.LABEL, Icon = "@drawable/icon", Theme = "@style/MyTheme", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : ToolbarActivity, BottomNavigationBar.Listeners.IOnMenuTabClickListener, ILocationListener
    {
        private BottomBar bottomBar;
        private Fragment currentFragment = null;

        private LocationManager locationManager;
        private string locationProvider;

        private HomeFragment homeFragment = null;

        private bool isFromSearchActivity = false;

        private HomeFragment HomeFragment
        {
            get
            {
                if (homeFragment == null) homeFragment = new HomeFragment();
                return homeFragment;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.Main, GetString(Resource.String.home));
            SetBottomBar(bundle);
            InitializeLocationManager();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (isFromSearchActivity)
            {
                MaterialSearchView.CloseSearch();
                isFromSearchActivity = false;
            }
            ScrobblerService.InitService(this.ApplicationContext);
            locationManager.RequestLocationUpdates(locationProvider, 2000, 1000, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
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
            this.isFromSearchActivity = true;
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

            BottomBarTab[] tabs = new BottomBarTab[4]
            {
                 new BottomBarTab(Resource.Drawable.ic_home_white_24dp, Resource.String.home),
                 new BottomBarTab(Resource.Drawable.ic_people_white_24dp, Resource.String.discover),
                 new BottomBarTab(Resource.Drawable.ic_favorite_white_24dp, Resource.String.match),
                 new BottomBarTab(Resource.Drawable.ic_settings_white_24dp, Resource.String.settings)
            };

            bottomBar.SetFixedInactiveIconColor("#44000000");

            bottomBar.SetItems(tabs);
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].Id = i;
                bottomBar.MapColorForTab(i, "#F44336");
            }

            bottomBar.SetOnMenuTabClickListener(this);
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
                    LaunchFragment(this.HomeFragment);
                    this.Toolbar.Title = GetString(Resource.String.home);
                    break;
                case 1:
                    LaunchFragment(new DiscoverFragment());
                    this.Toolbar.Title = GetString(Resource.String.discover);
                    break;
                case 2:
                    LaunchFragment(new MyMatchFragment());
                    this.Toolbar.Title = GetString(Resource.String.match);
                    break;
                case 3:
                    LaunchFragment(new SettingsFragment());
                    this.Toolbar.Title = GetString(Resource.String.settings);
                    break;
                default:
                    break;
            }
        }

        public void OnMenuTabReSelected(int menuItemId)
        {

        }

        void InitializeLocationManager()
        {
            locationManager = GetSystemService(LocationService) as LocationManager;
            locationProvider = "network";
        }

        private async void SetUserLocation(double longitude, double latitude)
        {
            await new ApiClient().PutUserLocation(CSFMPrefs.Prefs.GetString(CSFMApplication.Username, ""), longitude, latitude);
        }

        public void OnLocationChanged(Location location)
        {
            SetUserLocation(location.Longitude, location.Latitude);
        }

        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
           
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }
    }
}

