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
using Android.Support.V7.App;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using csfm_android.Ui.Adapters;
using csfm_android.Utils;

namespace csfm_android.Activities
{
    /// <summary>
    /// Activity displayed after submitting a query in the MainActivity material search view : Displays the results a the search.
    /// </summary>
    [Activity(Label = Configuration.LABEL, Icon = Configuration.ICON, WindowSoftInputMode = SoftInput.AdjustPan, Theme = Configuration.MAIN_THEME)]
    public class SearchActivity : ToolbarActivity
    {
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private SearchPagerAdapter pagerAdapter;

        public const string EXTRA_MESSAGE = "SearchActivity.Query";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            string query = Intent.GetStringExtra(EXTRA_MESSAGE)?.ToFirstUpperCases();

            base.OnCreate(savedInstanceState, Resource.Layout.search_activity, query);

            this.tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            this.viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            this.Toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_white_24dp);
            this.Toolbar.SetNavigationOnClickListener(new OnNavigationClickListener(this));

            InitViewPagerAndTabs(query);
        }

        private void InitViewPagerAndTabs(string query)
        {
            
            tabLayout.AddTab(tabLayout.NewTab().SetText(GetString(Resource.String.artists)));
            tabLayout.AddTab(tabLayout.NewTab().SetText(GetString(Resource.String.albums)));
            tabLayout.AddTab(tabLayout.NewTab().SetText(GetString(Resource.String.songs)));

            tabLayout.TabGravity = TabLayout.GravityFill;
            pagerAdapter = new SearchPagerAdapter(this, query);
            viewPager.Adapter = pagerAdapter;
            viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
            tabLayout.SetOnTabSelectedListener(new OnTabSelectedListener(viewPager, pagerAdapter));
            
        }

        /// <summary>
        /// New text is appended in the search view input
        /// </summary>
        /// <param name="newText">The new text displayed in the input</param>
        public override bool OnQueryTextChange(string newText)
        {
            return true;
        }

        /// <summary>
        /// The user submitted the text to search
        /// </summary>
        /// <param name="query">The text to search</param>
        /// <returns></returns>
        public override bool OnQueryTextSubmit(string query)
        {
            this.ToolbarTitle = query.ToFirstUpperCases();
            MaterialSearchView.CloseSearch();
            pagerAdapter.Update(query);
            return true;
        }


        /// <summary>
        /// Class used to manage Artists/Albums/Songs tabs
        /// </summary>
        private class OnTabSelectedListener : Java.Lang.Object, TabLayout.IOnTabSelectedListener
        {
            private ViewPager viewPager;
            private SearchPagerAdapter pagerAdapter;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="viewPager"></param>
            /// <param name="pagerAdapter"></param>
            public OnTabSelectedListener(ViewPager viewPager, SearchPagerAdapter pagerAdapter)
            {
                this.viewPager = viewPager;
                this.pagerAdapter = pagerAdapter;
            }

            public void OnTabReselected(TabLayout.Tab tab)
            {
                //
            }

            public void OnTabSelected(TabLayout.Tab tab)
            {
                viewPager.CurrentItem = tab.Position;
                pagerAdapter.GetItem(tab.Position);
            }

            public void OnTabUnselected(TabLayout.Tab tab)
            {
                //
            }
        }

        public class OnNavigationClickListener : Java.Lang.Object, View.IOnClickListener
        {
            public OnNavigationClickListener(Activity activity)
            {
                this.Activity = activity;
            }

            public Activity Activity { get; private set; }

            public void OnClick(View v)
            {
                Activity.Finish();
            }
        }
    }


}