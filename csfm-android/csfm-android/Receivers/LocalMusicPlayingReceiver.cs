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
    /// <summary>
    /// Music broadcast receiver to update the HomeFragment (via the HistoryAdapter) about the currently played song
    /// </summary>
    public class LocalMusicPlayingReceiver : BroadcastReceiver
    {
        private HistoryAdapter adapter;

        public LocalMusicPlayingReceiver(HistoryAdapter adapter)
        {
            this.adapter = adapter;
        }

        /// <summary>
        /// On broadcast receive
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra(Configuration.Music.EXTRA_PLAYING)) //Music action
            {
                if (intent.GetBooleanExtra(Configuration.Music.EXTRA_PLAYING, false))
                {
                    if (CSFMPrefs.Prefs.GetBoolean(CSFMApplication.IsScrobbling, true))
                    {
                        //Playing
                        adapter.Scrobble = intent.ToHistoryItem(true);
                    }
                    else
                    {
                        adapter.Scrobble = null;
                    }
                }
                else //Music paused or continuing
                {
                    //Pause
                    adapter.Scrobble = null;
                }
            }
            else
            {
                //Not about music
            }
        }


        /// <summary>
        /// Register on this broadcast receiver. Used by the history adapter
        /// </summary>
        /// <param name="context">The context that wants to register</param>
        /// <param name="adapter">The adapter to update on broadcast receive</param>
        public static void Register(Context context, HistoryAdapter adapter)
        {
            Register(context, new LocalMusicPlayingReceiver(adapter));
        }

        /// <summary>
        /// Register on this broadcast receiver
        /// </summary>
        /// <param name="context">The context that wants to register</param>
        /// <param name="receiver">broadcast receiver to register to</param>
        public static void Register(Context context, LocalMusicPlayingReceiver receiver)
        {
            context.RegisterReceiver(receiver, new IntentFilter().AddActions(Configuration.Music.MUSIC_ACTIONS));
        }
    }
}