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
using Android.Locations;

namespace csfm_android.Activities
{
    /// <summary>
    /// Abstract activity used by MainActivity and SearchActivity to take care of the toolbar (also containing the search view)
    /// </summary>
    public abstract class ToolbarActivity : AppCompatActivity
    {
        private Toolbar toolbar;

        /// <summary>
        /// Gets and sets the toolbar as SupportActionBar
        /// </summary>
        public Toolbar Toolbar
        {
            get
            {
                return toolbar;
            }

            set
            {
                toolbar = value;
                SetSupportActionBar(toolbar);
            }
        }

        //Changes the toolbar title
        protected string ToolbarTitle
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

        /// <summary>
        /// Search view
        /// </summary>
        public MaterialSearchView MaterialSearchView { get; set; }

        /// <summary>
        /// On creation of the activity
        /// </summary>
        /// <param name="savedInstanceState"></param>
        /// <param name="layout">Activity layout to use in SetContentView(int)</param>
        /// <param name="title">Title to display in the toolbar</param>
        protected void OnCreate(Bundle savedInstanceState, int layout, string title)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                SetContentView(layout);

                this.Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                this.ToolbarTitle = title; //Add the title on the toolbar
                this.Toolbar.NavigationIcon = GetDrawable(Resource.Drawable.ic_notifications_mfm); //Add the logo icon to the left of the toolbar
                this.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds); //Add Status Bar Color (colorPrimaryDark)
                this.MaterialSearchView = FindViewById<MaterialSearchView>(Resource.Id.material_design_search_view);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// On creation of the toolbar options menu : Initializes the MaterialSearchView
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            MaterialSearchView.MenuItem = this.Toolbar.Menu.FindItem(Resource.Id.action_search);
            MaterialSearchView.SearchViewListener = new SearchViewListener(this);
            MaterialSearchView.QueryTextListener = new QueryListener(this);
            MaterialSearchView.IsSetVoiceSearch = true;
            MaterialSearchView.Suggestions = SearchAdapter.SUGGESTIONS;
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// Do not use this function but use "OnCreate(Bundle, int, string)" instead.
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            throw new Exception("Use OnCreate(Bundle, int, string) to specifiy a resource Id and a title");
        }

        /// <summary>
        /// Handles the device "Back" button to include the search view in the history.
        /// </summary>
        public override void OnBackPressed()
        {
            if (this.MaterialSearchView.IsSearchOpen)
            {
                this.MaterialSearchView.CloseSearch();
                return;
            }

            base.OnBackPressed();
        }


        /// <summary>
        /// New text is appended in the search view input
        /// </summary>
        /// <param name="newText">The new text displayed in the input</param>
        /// <returns></returns>
        public abstract bool OnQueryTextChange(string newText);

        /// <summary>
        /// The user submitted the text to search
        /// </summary>
        /// <param name="query">The text to search</param>
        /// <returns></returns>
        public abstract bool OnQueryTextSubmit(string query);

        /// <summary>
        /// Called on voice request end : Sets the input of the search view to the specified query.
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == MaterialSearchView.REQUEST_VOICE && resultCode == Result.Ok)
            {
                IList<string> matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                if (matches != null && matches.Count > 0)
                {
                    string searchWord = matches.FirstOrDefault(m => !m.IsStringEmpty());
                    if (searchWord != null)
                    {
                        MaterialSearchView.SetQuery(searchWord, false);
                    }
                }
                return;
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        /// <summary>
        /// Material Search View Query Listener
        /// </summary>
        public class QueryListener : Java.Lang.Object, IOnQueryTextListener
        {
            private ToolbarActivity activity;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="activity"></param>
            public QueryListener(ToolbarActivity activity)
            {
                this.activity = activity;
            }

            /// <summary>
            /// When the input text changes
            /// </summary>
            /// <param name="newText"></param>
            /// <returns></returns>
            public bool OnQueryTextChange(string newText)
            {
                return this.activity.OnQueryTextChange(newText);
            }

            /// <summary>
            /// When the input text is submitted
            /// </summary>
            /// <param name="query"></param>
            /// <returns></returns>
            public bool OnQueryTextSubmit(string query)
            {
                return this.activity.OnQueryTextSubmit(query);
            }
        }

        /// <summary>
        /// Material Search View Open/Close listeners
        /// </summary>
        public class SearchViewListener : MaterialSearchView.ISearchViewListener
        {
            private ToolbarActivity a;

            public SearchViewListener(ToolbarActivity a)
            {
                this.a = a;
            }

            /// <summary>
            /// Material search view is closed
            /// </summary>
            public void OnSearchViewClosed()
            {
                this.a.Toolbar.Show(); //Show the toolbar
            }
        
            /// <summary>
            /// Material search view is opened
            /// </summary>
            public void OnSearchViewShown()
            {
                this.a.Toolbar.Hide(); //Hide the toolbar
            }
        }
    }
}