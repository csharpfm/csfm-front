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

namespace csfm_android.Ui.Holders
{
    public class HistoryHolder : RecyclerView.ViewHolder
    {

        public TextView SongName { get; private set; }

        public TextView SongArtist { get; private set; }

        public ImageView AlbumCover { get; private set; }

        public TextView Date { get; private set; }

        public HistoryHolder(View itemView) : base (itemView)
        {
            SongName = itemView.FindViewById<TextView>(Resource.Id.history_song_name);
            SongArtist = itemView.FindViewById<TextView>(Resource.Id.history_artist_name);
            AlbumCover = itemView.FindViewById<ImageView>(Resource.Id.history_image);
            Date = itemView.FindViewById<TextView>(Resource.Id.history_date);
        }
    }
}