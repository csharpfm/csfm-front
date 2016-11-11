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
    public class HistoryAdapter : RecyclerView.Adapter
    {
        private Context context;

        private History scrobble;
        public History Scrobble
        {
            get { return scrobble; }
            set
            {
                if (scrobble == value) return;
                scrobble = value;
                NotifyDataSetChanged();
            }
        }

        private List<History> data;
        public List<History> Data
        {
            get { return data; }
            set
            {
                data = value;
                NotifyDataSetChanged();
            }
        }

        public override int ItemCount
        {
            get
            {
                return Data != null ? Data.Count + (Scrobble != null ? 1 : 0) : 0;
            }
        }

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

        public HistoryAdapter(Context context, List<History> history)
        {
            this.context = context;
            this.data = history;
            LocalMusicPlayingReceiver.Register(context, this);
            MaterialSearchView.AddSuggestions(Data.ToTrackNames());
            MaterialSearchView.AddSuggestions(Data.ToAlbumNames());
            MaterialSearchView.AddSuggestions(Data.ToArtistNames());
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.history_item, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            HistoryHolder holder = new HistoryHolder(itemView);
            return holder;
        }

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