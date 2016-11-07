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

namespace csfm_android.Ui.Adapters
{
    public class HistoryAdapter : RecyclerView.Adapter
    {

        private List<History> historic;

        private Context context;
        private object hhistory;

        public HistoryAdapter(Context context, List<History> historic)
        {
            this.context = context;
            this.historic = historic;
            MaterialSearchView.AddSuggestions(historic.ToTrackNames());
            MaterialSearchView.AddSuggestions(historic.ToAlbumNames());
            MaterialSearchView.AddSuggestions(historic.ToArtistNames());
        }

        public override int ItemCount
        {
            get
            {
                return historic.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (position < ItemCount)
            {
                History history = historic[position];

                if (history != null)
                {
                    HistoryHolder historyHolder = holder as HistoryHolder;

                    historyHolder.SongName.Text = history.Track.Name;
                    historyHolder.SongArtist.Text = history.Track.Artist.Name;

                    Picasso.With(context)
                       .Load(history.Track.Album.Image)
                       .Transform(new CircleTransform())
                       .Into(historyHolder.AlbumCover);

                    historyHolder.Date.Text = history.Date.ToString();
                }
            }
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
    }
}