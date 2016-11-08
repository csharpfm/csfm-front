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
using csfm_android.Services;
using Android.Provider;
using csfm_android.Utils;

namespace csfm_android.Receivers
{
    public class MusicPlayingReceiver : BroadcastReceiver
    {
        public const string PLAYING_EXTRA = "playing";
        public const string ARTIST_EXTRA = "artist";
        public const string ALBUM_EXTRA = "album";
        public const string TRACK_EXTRA = "track";

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra(PLAYING_EXTRA)) //Music action
            {
                if (intent.GetBooleanExtra(PLAYING_EXTRA, false) && intent.GetLongExtra("position", 1) == 0) //Music is at 00:00 and is playing
                {
                    ScrobblerService.SendScrobble(intent.GetArtist(), intent.GetAlbum(), intent.GetTrack(), context);
                }
                else //Music paused or continuing
                {
                    //ScrobblerService.StopScrobble(context, intent.GetArtist(), intent.GetAlbum(), intent.GetTrack());
                }
            }
            else
            {
                //ScrobblerService.StopScrobble(context);
            }
        }
    }
}