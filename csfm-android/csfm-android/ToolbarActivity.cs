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
using SearchView = Android.Support.V7.Widget.SearchView;
using static Android.Support.V7.Widget.SearchView;

namespace csfm_android
{
    public abstract class ToolbarActivity : AppCompatActivity
    {

        public Android.Support.V7.Widget.Toolbar Toolbar { get; private set; }

        public IMenuItem SearchItem { get; private set; }
        private SearchView searchView;
        public SearchView SearchView
        {
            get
            {
                return searchView;
            }

            private set
            {
                searchView = value;
                if (searchView != null)
                {
                    OnSearchViewSet();
                }
            }
        }

        protected void OnCreate(Bundle savedInstanceState, int layout, string title)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                SetContentView(layout);

                this.Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(this.Toolbar);
                this.Toolbar.Title = title;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            this.Toolbar.MenuItemClick += Toolbar_MenuItemClick;
            SearchItem = this.Toolbar.Menu.FindItem(Resource.Id.action_search);
            SearchView = SearchItem.ActionView.JavaCast<SearchView>();
            SearchView.SetOnQueryTextListener(new QueryListener(this));

            return base.OnCreateOptionsMenu(menu);
        }

        private void Toolbar_MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            Console.WriteLine("Menu Item Click -----------------");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            throw new Exception("Use OnCreate(Bundle, int) to specifiy a resource Id");
        }


        public abstract bool OnQueryTextChange(string newText);
        public abstract bool OnQueryTextSubmit(string query);

        public abstract void OnSearchViewSet();

        public class QueryListener : Java.Lang.Object, IOnQueryTextListener
        {
            private ToolbarActivity activity;

            public QueryListener(ToolbarActivity activity)
            {
                this.activity = activity;
            }

            public bool OnQueryTextChange(string newText)
            {
                return this.activity.OnQueryTextChange(newText);
            }

            public bool OnQueryTextSubmit(string query)
            {
                return this.activity.OnQueryTextSubmit(query);
            }
        }


    }
}