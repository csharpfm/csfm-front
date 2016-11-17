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
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using csfm_android.Api.Model;
using csfm_android.Fragments;
using csfm_android.Activities;

namespace csfm_android.Ui.Adapters
{
    /// <summary>
    /// Tabs adapter on SearchActivity
    /// </summary>
    public class SearchPagerAdapter : FragmentPagerAdapter
    {
        /// <summary>
        /// Search words
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// List of fragments. Will be created on first access.
        /// </summary>
        public List<Lazy<SearchFragment>> fragments;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="query">The words to search for</param>
        public SearchPagerAdapter(ToolbarActivity activity, string query) : base(activity.SupportFragmentManager)
        {
            this.Query = query;
            this.fragments = new List<Lazy<SearchFragment>>(3)
            {
                new Lazy<SearchFragment>(() => new SearchArtistFragment(this)),
                new Lazy<SearchFragment>(() => new SearchAlbumFragment(this)),
                new Lazy<SearchFragment>(() => new SearchTrackFragment(this))
            };
        }

        /// <summary>
        /// Number of tabs
        /// </summary>
        public override int Count
        {
            get
            {
                return fragments != null ? fragments.Count : 0;
            }
        }

        /// <summary>
        /// Get the fragment to display
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Fragment GetItem(int position)
        {
            if (position >= 0 && position < fragments.Count)
            {
                return fragments[position].Value;
            }
            return null;
        }

        /// <summary>
        /// In case of a new search in the SearchActivity MaterialSearchView : Update the displayed tabs results.
        /// </summary>
        /// <param name="query"></param>
        public void Update(string query)
        {
            this.Query = query;
            foreach(Lazy<SearchFragment> f in fragments)
            {
                if (f.IsValueCreated)
                {
                    f.Value.Update(query);
                }
            }
        }
    }
}