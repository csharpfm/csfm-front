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


        private HistoryAdapter adapter;

        public LocalMusicPlayingReceiver(HistoryAdapter adapter)
        {
            this.adapter = adapter;
        }

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


        public static void Register(Context context, HistoryAdapter adapter)
        {
            Register(context, new LocalMusicPlayingReceiver(adapter));
        }

        public static void Register(Context context, LocalMusicPlayingReceiver receiver)
        {
            context.RegisterReceiver(receiver, new IntentFilter().AddActions(Configuration.Music.MUSIC_ACTIONS));
        }
    }
}