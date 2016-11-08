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
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "com.android.music.metachanged", "com.android.music.playstatechanged", "com.android.music.playbackcomplete", "com.android.music.queuechanged" })]
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
                if (intent.GetBooleanExtra(PLAYING_EXTRA, false)) //Music is at 00:00 and is playing
                {
                    if (intent.GetPosition(1) == 0)
                    {
                        
                    }
                    Console.WriteLine("Playing");
                    ScrobblerService.SendScrobble(intent.GetArtist(), intent.GetAlbum(), intent.GetTrack(), context);
                }
                else //Music paused or continuing
                {
                    Console.WriteLine("Pause");
                    ScrobblerService.StopScrobble(context, intent.GetArtist(), intent.GetAlbum(), intent.GetTrack());
                }
            }
            else
            {
                Console.WriteLine("Something else");
                //ScrobblerService.StopScrobble(context);
            }
        }
    }
}