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
using Toolbar = Android.Support.V7.Widget.Toolbar;
using static Android.Support.V7.Widget.SearchView;
using static Android.Views.View;
using csfm_android.Utils.MaterialDesignSearchView;
using Android.Speech;
using csfm_android.Utils;

namespace csfm_android.Activities
{
    public abstract class ToolbarActivity : AppCompatActivity
    {

        public Toolbar Toolbar { get; private set; }

        public string ToolbarTitle
        {
            get
            {
                return Toolbar.Title;
            }

            set
            {
                this.Title = value;
                this.Toolbar.Title = value;
            }
        }



        public IMenuItem SearchItem { get; private set; }

        private MaterialSearchView materialSearchView;
        public MaterialSearchView MaterialSearchView
        {
            get
            {
                return materialSearchView;
            }
            set
            {
                materialSearchView = value;
                if (materialSearchView != null)
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
                this.ToolbarTitle = title;
                this.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds); //Add Status Bar Color (colorPrimaryDark)
                this.MaterialSearchView = FindViewById<MaterialSearchView>(Resource.Id.material_design_search_view);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            this.Toolbar.MenuItemClick += Toolbar_MenuItemClick;
            SearchItem = this.Toolbar.Menu.FindItem(Resource.Id.action_search);
            MaterialSearchView.MenuItem = SearchItem;
            MaterialSearchView.SearchViewListener = new SearchViewListener(this);
            MaterialSearchView.QueryTextListener = new QueryListener(this);
            MaterialSearchView.IsSetVoiceSearch = true;
            MaterialSearchView.Suggestions = SearchAdapter.SUGGESTIONS;
            return base.OnCreateOptionsMenu(menu);
        }

        private void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            Console.WriteLine("Menu Item Click -----------------");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            throw new Exception("Use OnCreate(Bundle, int, string) to specifiy a resource Id");
        }

        public override void OnBackPressed()
        {
            if (this.MaterialSearchView.IsSearchOpen)
            {
                this.MaterialSearchView.CloseSearch();
                return;
            }

            base.OnBackPressed();
        }


        public abstract bool OnQueryTextChange(string newText);
        public abstract bool OnQueryTextSubmit(string query);

        public abstract void OnSearchViewSet();


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == MaterialSearchView.REQUEST_VOICE && resultCode == Result.Ok)
            {
                IList<string> matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                if (matches != null && matches.Count > 0)
                {
                    string searchWord = matches[0];
                    if (!searchWord.IsStringEmpty())
                    {
                        MaterialSearchView.SetQuery(searchWord, false);
                    }
                }

                return;

            }
            base.OnActivityResult(requestCode, resultCode, data);
        }


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

        public class SearchViewListener : MaterialSearchView.ISearchViewListener
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