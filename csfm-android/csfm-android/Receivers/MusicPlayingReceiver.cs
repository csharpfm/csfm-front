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
    [IntentFilter(new[] { Configuration.Music.ACTION_METACHANGED, Configuration.Music.ACTION_PLAYBACKCOMPLETE, Configuration.Music.ACTION_PLAYSTATECHANGED, Configuration.Music.ACTION_QUEUECHANGED })]
    public class MusicPlayingReceiver : BroadcastReceiver
    { 

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra(Configuration.Music.EXTRA_PLAYING)) //Music action
            {
                if (intent.GetBooleanExtra(Configuration.Music.EXTRA_PLAYING, false)) //Music is at 00:00 and is playing
                {
                    Console.WriteLine("Playing");
                    if (intent.GetPosition(1) == 0)
                    {
                        Console.WriteLine("Scrobbled");
                        ScrobblerService.SendScrobble(intent.GetArtist(), intent.GetAlbum(), intent.GetTrack(), context);
                    }
                }
                else //Music paused or continuing
                {
                    Console.WriteLine("Pause");
                    ScrobblerService.StopScrobble(context);
                }
            }
            else
            {
                //Not about music
            }
        }
    }
}