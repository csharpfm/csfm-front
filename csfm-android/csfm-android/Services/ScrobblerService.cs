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
using csfm_android.Receivers;
using csfm_android.Notifications;
using Android.Provider;
using csfm_android.Utils;
using csfm_android.Api;

namespace csfm_android.Services
{
    /// <summary>
    /// Scrobbler service used to scrobble music via the MatchFM API (woken up by MusicPlayingReceiver (Broadcast Receiver)
    /// </summary>
    [Service(Enabled = true)]
    public class ScrobblerService : Service
    {
        public const string ACTION_INIT = "Init";
        public const string ACTION_SCROBBLE = "Scrobble";
        public const string ACTION_STOP_SCROBBLE = "Stop";
        public const string ACTION_CLOSE = "Close";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
           if (intent?.Action != null)
            {
                
                switch(intent.Action) //Choose action depending on intent's specified action
                {
                    case ACTION_SCROBBLE:
                        Scrobble(intent);
                        break;
                    case ACTION_STOP_SCROBBLE:
                        StopScrobbling(intent);
                        break;
                    default:
                        break;
                }

            }
            return StartCommandResult.NotSticky;
        }

        /// <summary>
        /// Scrobble a new song
        /// </summary>
        /// <param name="intent"></param>
        private void Scrobble(Intent intent)
        {
            if (CSFMPrefs.Prefs.GetBoolean(CSFMApplication.IsScrobbling, true))
            {
                string artist = intent.GetArtist();
                string album = intent.GetAlbum();
                string track = intent.GetTrack();
                long durationToEnd = intent.GetDuration(0) - intent.GetPosition(0);
                durationToEnd = durationToEnd < 0 ? 0 : durationToEnd;
                DateTime endTime = DateTime.Now.AddMilliseconds(durationToEnd); ;
                string albumArt = MusicLibrary.GetAlbumArt(artist, album, CSFMApplication.Context)?.FirstOrDefault();


                Action callback = () => AppNotificationManager.SendNotification(artist, album, track, this, this.ApplicationContext);

                ApiClient client = new ApiClient();
                string username = client.RetrieveUsername();
                if (username != null)
                    client.PostHistory(username, artist, album, track, callback);


                ScrobblePrefs.Save(artist, album, track, endTime.Ticks);
            }
        }

        /// <summary>
        /// Removes the last scrobble from shared preferences (internal memory)
        /// </summary>
        /// <param name="intent"></param>
        public void StopScrobbling(Intent intent)
        {
            ScrobblePrefs.Clear();
        }

        /// <summary>
        /// Wake up the service to send a scrobble
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="context"></param>
        public static void SendScrobble(Intent intent, Context context)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_SCROBBLE);
            serviceIntent.PutExtras(intent.Extras);
            context.StartService(serviceIntent);
        }

        /// <summary>
        /// Wake up the service to stop the scrobble
        /// </summary>
        /// <param name="context"></param>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="track"></param>
        public static void StopScrobble(Context context, string artist = null, string album = null, string track = null)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_STOP_SCROBBLE);
            context.StartService(serviceIntent);
        }

        /// <summary>
        /// On service destroy, clear the scrobble (so the internal memory info is never outdated)
        /// </summary>
        public override void OnDestroy()
        {
            ScrobblePrefs.Clear();
            base.OnDestroy();
        }
    }
}