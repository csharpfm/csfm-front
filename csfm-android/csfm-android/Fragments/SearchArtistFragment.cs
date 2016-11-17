using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using csfm_android.Api.Model;
using csfm_android.Ui.Adapters;
using csfm_android.Utils.MaterialDesignSearchView;

namespace csfm_android.Fragments
{
    /// <summary>
    /// Fragment used to display the artists corresponding to the search query
    /// </summary>
    public class SearchArtistFragment : SearchFragment
    {
        private SearchArtistAdapter adapter;

        /// <summary>
        /// RecyclerView Adapter
        /// </summary>
        protected SearchArtistAdapter Adapter
        {
            get
            {
                if (adapter == null && recyclerView != null)
                {
                    SetRecyclerViewAdapter(recyclerView, noResult);
                }
                return adapter;
            }

            set
            {
                SetRecyclerViewAdapter(recyclerView, value);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pagerAdapter"></param>
        public SearchArtistFragment(SearchPagerAdapter pagerAdapter) : base(pagerAdapter)
        {

        }

        /// <summary>
        /// Initializes the recycler view layout manager
        /// </summary>
        /// <param name="recyclerView"></param>
        protected override void SetRecyclerViewLayoutManager(RecyclerView recyclerView)
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
        }

        /// <summary>
        /// Initializes the recycler view with the adapter
        /// </summary>
        /// <param name="recyclerView"></param>
        /// <param name="adapter"></param>
        private void SetRecyclerViewAdapter(RecyclerView recyclerView, SearchArtistAdapter adapter)
        {
            recyclerView.SetAdapter(this.adapter = adapter);
        }

        /// <summary>
        /// Initializes the recycler view with an empty adapter
        /// </summary>
        /// <param name="recyclerView"></param>
        /// <param name="noResult"></param>
        protected override void SetRecyclerViewAdapter(RecyclerView recyclerView, View noResult)
        {
            SetRecyclerViewAdapter(recyclerView, new SearchArtistAdapter(this.Context, new List<History>()));
        }

        /// <summary>
        /// Updates the search results
        /// </summary>
        /// <param name="callback">Callback invoked at the end of the update</param>
        protected override void Update(Action callback)
        {
            //Nothing to do. History list has to be updated on the HomeFragment
            callback();
        }

        /// <summary>
        /// Searches for the new query in the history list
        /// </summary>
        /// <param name="query">New query</param>
        /// <param name="callback">Callback invoked at the end of the input</param>
        protected override void Update(string query, Action callback)
        {
            List<History> history = MaterialSearchView.History;
            string[] words = query.ToLower().Trim().Split(' ');
            List<History> artists = history.Where(h => words.All(w => h.Track.Album.Artist.Name.ToLower().Trim().Contains(w))).ToList();
            Adapter.Data = artists;
            callback();
        }
    }
}