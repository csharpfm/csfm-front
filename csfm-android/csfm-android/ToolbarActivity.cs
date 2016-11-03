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
using static Android.Views.View;
using csfm_android.Utils.MaterialDesignSearchView;

namespace csfm_android
{
    public abstract class ToolbarActivity : AppCompatActivity
    {

        public Android.Support.V7.Widget.Toolbar Toolbar { get; private set; }

        public IMenuItem SearchItem { get; private set; }
        //private SearchView searchView;
        /*public SearchView SearchView
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
        }*/
        public MaterialSearchView MaterialSearchView { get; set; }

        protected void OnCreate(Bundle savedInstanceState, int layout, string title)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                SetContentView(layout);

                this.Toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(this.Toolbar);
                this.Toolbar.Title = title;
                this.Title = title;
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
            /*SearchView = SearchItem.ActionView.JavaCast<SearchView>();
            SearchView.SetOnQueryTextListener(new QueryListener(this));
            SearchView.SetOnSearchClickListener(new SearchClickListener(this));
            SearchView.SetOnCloseListener(new SearchCloseListener(this));*/

            MaterialSearchView = FindViewById<MaterialSearchView>(Resource.Id.material_design_search_view);
            MaterialSearchView.SetMenuItem(SearchItem);
            MaterialSearchView.SetOnSearchViewListener(new SearchViewListener(this));

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

        public class SearchClickListener : Java.Lang.Object, IOnClickListener
        {
            public ToolbarActivity Activity { get; private set; }

            public SearchClickListener(ToolbarActivity activity)
            {
                this.Activity = activity;
            }


            public void OnClick(View v)
            {
                Activity.Toolbar.SetNavigationIcon(Resource.Drawable.ic_arrow_back_white_24dp);
            }
        }

        public class SearchCloseListener : Java.Lang.Object, IOnCloseListener
        {
            public ToolbarActivity Activity { get; private set; }

            public SearchCloseListener(ToolbarActivity activity)
            {
                this.Activity = activity;
            }

            public bool OnClose()
            {
                Activity.Toolbar.SetNavigationIcon(Android.Resource.Color.Transparent);
                Activity.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                return false;
            }
        }

        public class SearchViewListener : ISearchViewListener
        {
            private ToolbarActivity a;

            public SearchViewListener(ToolbarActivity a)
            {
                this.a = a;
            }

            public void OnSearchViewClosed()
            {
                this.a.Toolbar.Visibility = ViewStates.Visible;
            }

            public void OnSearchViewShown()
            {
                this.a.Toolbar.Visibility = ViewStates.Gone;
            }
        }


    }
}