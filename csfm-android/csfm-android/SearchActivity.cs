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

namespace csfm_android
{
    [Activity(Label = Configuration.LABEL, Icon = "@drawable/icon", WindowSoftInputMode = SoftInput.AdjustPan, Theme = "@style/MyTheme")]
    public class SearchActivity : ToolbarActivity, View.IOnClickListener
    {
        //private ResourceButton[] btn = new ResourceButton[3];
        //private Button btn_unfocus;
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private SearchPagerAdapter pagerAdapter;

        public const string EXTRA_MESSAGE = "SearchActivity.Query";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState, Resource.Layout.search_activity);
            //this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //this.SupportActionBar.SetDisplayShowHomeEnabled(true);
            this.tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            this.viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);

            this.Toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_white_24dp);
            this.Toolbar.SetNavigationOnClickListener(new OnNavigationClickListener(this));

            InitViewPagerAndTabs();

            //btn[0] = new ResourceButton(Resource.Id.btn0, this);
            //btn[1] = new ResourceButton(Resource.Id.btn1, this);
            //btn[2] = new ResourceButton(Resource.Id.btn2, this);

            //for (int i = 0; i < btn.Length; i++)
            //{
            //    btn[i].Button.SetBackgroundColor(Color.Rgb(207, 207, 207));
            //    btn[i].Button.SetOnClickListener(this);

            //}
            
        }

        public override void OnSearchViewSet()
        {
            this.SearchView.Iconified = false;
            this.SearchView.SetQuery(Intent.GetStringExtra(EXTRA_MESSAGE), false);
            this.SearchView.ClearFocus();
        }

        private void InitViewPagerAndTabs()
        {
            
            tabLayout.AddTab(tabLayout.NewTab().SetText("Artistes"));
            tabLayout.AddTab(tabLayout.NewTab().SetText("Albums"));
            tabLayout.AddTab(tabLayout.NewTab().SetText("Chansons"));

            tabLayout.TabGravity = TabLayout.GravityFill; //ModeScrollable
            pagerAdapter = new SearchPagerAdapter(this);
            viewPager.Adapter = pagerAdapter;
            viewPager.AddOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
            tabLayout.SetOnTabSelectedListener(new OnTabSelectedListener(viewPager, pagerAdapter));
            
        }

        public void OnClick(View v)
        {
            //int id = v.Id;
            //SetFocus(btn.FirstOrDefault(b => b.ResourceId == id)?.Button);
           
        }

        private void SetFocus(Button selected)
        {
            //Console.WriteLine(selected);
            //if (selected == null) return;

            //if (this.btn_unfocus != null)
            //{
            //    this.btn_unfocus.SetTextColor(Color.Rgb(49, 50, 51));
            //    this.btn_unfocus.SetBackgroundColor(Color.Rgb(207, 207, 207));
            //}

            //selected.SetTextColor(Color.Rgb(255, 255, 255));
            //selected.SetBackgroundColor(Color.Rgb(3, 106, 150));

            //this.btn_unfocus = selected;
        }

        public override bool OnQueryTextChange(string newText)
        {
            return true;
        }

        public override bool OnQueryTextSubmit(string query)
        {
            return true; //TODO
        }

        private class ResourceButton
        {
            public Button Button { get; set; }
            public int ResourceId { get; set; }

            public ResourceButton(int id, Activity activity)
            {
                this.ResourceId = id;
                this.Button = activity.FindViewById<Button>(this.ResourceId);
            }
        }

        private class OnTabSelectedListener : Java.Lang.Object, TabLayout.IOnTabSelectedListener
        {
            public ViewPager ViewPager { get; private set; }
            public SearchPagerAdapter PagerAdapter { get; private set; }

            public OnTabSelectedListener(ViewPager viewPager, SearchPagerAdapter pagerAdapter)
            {
                this.ViewPager = viewPager;
                this.PagerAdapter = pagerAdapter;
            }


            public void OnTabReselected(TabLayout.Tab tab)
            {
                //  TODO : Go to top
            }

            public void OnTabSelected(TabLayout.Tab tab)
            {
                ViewPager.CurrentItem = tab.Position;
                PagerAdapter.GetItem(tab.Position);
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