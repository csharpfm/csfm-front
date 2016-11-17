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
using Android.Support.V7.Widget;
using csfm_android.Api.Model;
using csfm_android.Ui.Holders;
using Square.Picasso;
using csfm_android.Ui.Utils;
using csfm_android.Utils.MaterialDesignSearchView;
using csfm_android.Utils;
using csfm_android.Receivers;

namespace csfm_android.Ui.Adapters
{
    /// <summary>
    /// HomeFragment RecyclerView's adapter
    /// </summary>
    public class HistoryAdapter : RecyclerView.Adapter
    {
        private Context context;

        private History scrobble;

        /// <summary>
        /// Special rotating item : Currently played song
        /// </summary>
        public History Scrobble
        {
            get { return scrobble; }
            set
            {
                if (scrobble == value) return;
                if (scrobble != null && value == null)
                {
                    var firstItem = data.FirstOrDefault();
                    if (firstItem != null && (firstItem.Track?.Name == scrobble.Track?.Name && firstItem.Track?.Album?.Name == scrobble.Track?.Album?.Name && firstItem.Track?.Album?.Artist?.Name == scrobble.Track?.Album?.Artist?.Name))
                    {
                        //Item already displayed
                    }
                    else
                    {
                        scrobble.IsScrobbling = false;
                        data.Insert(0, scrobble);
                    }
                }
                scrobble = value;
                NotifyDataSetChanged();
            }
        }

        private List<History> data;
        /// <summary>
        /// History items (the Scrobble item will also be displayed before these items)
        /// Updates the view on new data set.
        /// </summary>
        public List<History> Data
        {
            get { return data; }
            set
            {
                data = value;
                MaterialSearchView.History = value;
                MaterialSearchView.SetSuggestions(value);
                NotifyDataSetChanged();
            }
        }

        /// <summary>
        /// Number of items in the list (including the special scrobble item)
        /// </summary>
        public override int ItemCount
        {
            get
            {
                return Data != null ? Data.Count + (Scrobble != null ? 1 : 0) : 0;
            }
        }

        /// <summary>
        /// Indexer to the list of items. This includes the Scrobble item as element 0 if not null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private History this[int index]
        {
            get
            {
                if (scrobble == null) return Data[index];

                //Scrobble != null
                if (index == 0)
                {
                    return scrobble;
                }
                else
                {
                    return Data[index - 1];
                }
            }
        }

        /// <summary>
        /// Constructor. Also registers to the LocalMusicPlayingReceiver
        /// </summary>
        /// <param name="context"></param>
        /// <param name="history"></param>
        public HistoryAdapter(Context context, List<History> history)
        {
            this.context = context;
            this.data = history;
            LocalMusicPlayingReceiver.Register(context, this);
            MaterialSearchView.History = history;
            MaterialSearchView.SetSuggestions(history);
        }

        /// <summary>
        /// On item view creation
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.history_item, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            HistoryHolder holder = new HistoryHolder(itemView);
            return holder;
        }

        /// <summary>
        /// On new information to bind on the view
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position < ItemCount)
            {
                History history = this[position];

                if (history != null)
                {
                    (holder as HistoryHolder)?.Bind(history);
                }
            }
        }

    }
}