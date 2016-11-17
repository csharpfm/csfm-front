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
    /// <summary>
    /// Broadcast receiver used by the Scrobbling service. This receiver is specified in the manifest (via annotations below) and is therefore launched automatically on broadcasts listed on annotations below
    /// </summary>

    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Configuration.Music.ACTION_METACHANGED, Configuration.Music.ACTION_PLAYBACKCOMPLETE, Configuration.Music.ACTION_PLAYSTATECHANGED, Configuration.Music.ACTION_QUEUECHANGED })]
    public class MusicPlayingReceiver : BroadcastReceiver
    { 
        /// <summary>
        /// On broadcast receive
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra(Configuration.Music.EXTRA_PLAYING)) //Music action
            {
                if (intent.GetBooleanExtra(Configuration.Music.EXTRA_PLAYING, false)) //Music is playing
                {
                    if (intent.GetPosition(1) == 0) //Music is at 00:00
                    {
                        ScrobblerService.SendScrobble(intent, context);
                    }
                }
                else //Music paused or continuing
                {
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