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
    /// <summary>
    /// Main activity of the application. Regroups the Home, Discover, MyMatch and Settings fragments
    /// </summary>
    [Activity(Label = Configuration.LABEL, Icon = Configuration.ICON, Theme = Configuration.MAIN_THEME, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : ToolbarActivity, BottomNavigationBar.Listeners.IOnMenuTabClickListener, ILocationListener
    {
        private BottomBar bottomBar;
        private Fragment currentFragment = null;

        private LocationManager locationManager;
        private string locationProvider;

        private bool isFromSearchActivity = false;

        private Lazy<HomeFragment> homeFragmentLazy = new Lazy<HomeFragment>(() => new HomeFragment());

        /// <summary>
        /// Get the current HomeFragment. Initializes it on first use.
        /// </summary>
        private HomeFragment HomeFragment
        {
            get { return homeFragmentLazy.Value; }
        }

        private Lazy<DiscoverFragment> discoverFragmentLazy = new Lazy<DiscoverFragment>(() => new DiscoverFragment());
        /// <summary>
        /// Get the current DiscoverFragment. Initializes it on first use.
        /// </summary>
        private DiscoverFragment DiscoverFragment
        {
            get { return discoverFragmentLazy.Value; }
        }

        private Lazy<MyMatchFragment> myMatchFragmentLazy = new Lazy<MyMatchFragment>(() => new MyMatchFragment());
        /// <summary>
        /// Get the current MyMatchFragment. Initializes it on first use.
        /// </summary>
        private MyMatchFragment MyMatchFragment
        {
            get { return myMatchFragmentLazy.Value; }
        }

        private Lazy<SettingsFragment> settingsFragmentLazy = new Lazy<SettingsFragment>(() => new SettingsFragment());
        /// <summary>
        /// Get the current SettingsFragment. Initializes it on first use.
        /// </summary>
        private SettingsFragment SettingsFragment
        {
            get { return settingsFragmentLazy.Value; }
        }

        /// <summary>
        /// On creation of the activity
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.Main, GetString(Resource.String.home));
            SetBottomBar(bundle);
            InitializeLocationManager();
        }
        
        /// <summary>
        /// On resuming the activity
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            if (isFromSearchActivity) //When coming back from SearchActivity, closing the currently opened Search Bar.
            {
                MaterialSearchView.CloseSearch();
                isFromSearchActivity = false;
            }
            locationManager.RequestLocationUpdates(locationProvider, Configuration.Location.MIN_TIME, Configuration.Location.MIN_DISTANCE, this);
        }

        /// <summary>
        /// On pausing the activity
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        /// <summary>
        /// When the user types new text in the search view
        /// </summary>
        /// <param name="newText"></param>
        /// <returns></returns>
        public override bool OnQueryTextChange(string newText)
        {
            return true;
        }

        /// <summary>
        /// When the user submits the content to search in the search view
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override bool OnQueryTextSubmit(string query)
        {
            Intent intent = new Intent(this, typeof(SearchActivity));
            intent.PutExtra(SearchActivity.EXTRA_MESSAGE, query);
            this.isFromSearchActivity = true; //Used on OnResume()
            this.StartActivity(intent); //Start search activity with the query as intent extra
            return true;
        }

        /// <summary>
        /// Prepares the bottom bar
        /// </summary>
        /// <param name="bundle"></param>
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

            bottomBar.SetFixedInactiveIconColor(Configuration.BottomBar.INACTIVE_ICON_COLOR);

            bottomBar.SetItems(tabs);
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].Id = i;
                bottomBar.MapColorForTab(i, Configuration.BottomBar.BOTTOM_BAR_BACKGROUND_COLOR);
            }

            bottomBar.SetOnMenuTabClickListener(this);
        }

        /// <summary>
        /// Replaces the current fragment
        /// </summary>
        /// <param name="fragment">New fragment to show</param>
        /// <param name="title">New toolbar title</param>
        private void LaunchFragment(Fragment fragment, string title)
        {
            FragmentTransaction fragmentTransaction = this.FragmentManager.BeginTransaction();
            if (currentFragment != null)
            {
                fragmentTransaction.Remove(currentFragment);
            }

            currentFragment = fragment;

            fragmentTransaction.Add(Resource.Id.mainContainer, fragment);
            fragmentTransaction.Commit();

            this.ToolbarTitle = title;
        }

        /// <summary>
        /// When selecting a new item in the bottom bar : Shows a different fragment
        /// </summary>
        /// <param name="menuItemId"></param>
        public void OnMenuTabSelected(int menuItemId)
        {
            switch(menuItemId)
            {
                case 0:
                    LaunchFragment(this.HomeFragment, GetString(Resource.String.home));
                    break;
                case 1:
                    LaunchFragment(this.DiscoverFragment, GetString(Resource.String.discover));
                    break;
                case 2:
                    LaunchFragment(this.MyMatchFragment, GetString(Resource.String.match));
                    break;
                case 3:
                    LaunchFragment(this.SettingsFragment, GetString(Resource.String.settings));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// When a bottom bar tab is reselected
        /// </summary>
        /// <param name="menuItemId"></param>
        public void OnMenuTabReSelected(int menuItemId)
        {
            //Nothing to do
        }

        /// <summary>
        /// Inits the location manager
        /// </summary>
        void InitializeLocationManager()
        {
            locationManager = GetSystemService(LocationService) as LocationManager;
            locationProvider = Configuration.Location.LOCATION_PROVIDER;
        }

        /// <summary>
        /// API Request to set the user location
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        private void SetUserLocation(double longitude, double latitude)
        {
            new ApiClient().PutUserLocation(CSFMPrefs.Username, longitude, latitude);
        }

        /// <summary>
        /// When user location changes, sends an API Request to set the new location
        /// </summary>
        /// <param name="location"></param>
        public void OnLocationChanged(Location location)
        {
            SetUserLocation(location.Longitude, location.Latitude);
        }

        /// <summary>
        /// In case location goes disabled
        /// </summary>
        /// <param name="provider"></param>
        public void OnProviderDisabled(string provider)
        {
            //   
        }

        /// <summary>
        /// When location goes enabled
        /// </summary>
        /// <param name="provider"></param>
        public void OnProviderEnabled(string provider)
        {
           //
        }

        /// <summary>
        /// When location provider status changes
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="status"></param>
        /// <param name="extras"></param>
        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //
        }
    }
}

