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
using csfm_android.Ui.Adapters;
using csfm_android.Api.Model;
using csfm_android.Utils;

namespace csfm_android.Receivers
{
    public class LocalMusicPlayingReceiver : BroadcastReceiver
    {
        public const string PLAYING_EXTRA = "playing";
        public const string ARTIST_EXTRA = "artist";
        public const string ALBUM_EXTRA = "album";
        public const string TRACK_EXTRA = "track";

        private HistoryAdapter adapter;

        public LocalMusicPlayingReceiver(HistoryAdapter adapter)
        {
            this.adapter = adapter;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra(PLAYING_EXTRA)) //Music action
            {
                if (intent.GetBooleanExtra(PLAYING_EXTRA, false))
                {
                    //Playing
                    adapter.AddScrobble(intent.ToHistoryItem(true));
                }
                else //Music paused or continuing
                {
                    //Pause
                    adapter.AddScrobble(null);
                }
            }
            else
            {
                adapter.AddScrobble(null);
            }
        }
    }
}