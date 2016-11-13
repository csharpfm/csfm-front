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
                
                switch(intent.Action)
                {
                    case ACTION_SCROBBLE:
                        Scrobble(intent);
                        break;
                    case ACTION_STOP_SCROBBLE:
                        StopScrobbling(intent);
                        break;
                    case ACTION_CLOSE:
                        Close(intent);
                        break;
                    default:
                        break;
                }

            }
            return StartCommandResult.NotSticky;
        }

        private void Scrobble(Intent intent)
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

        private void Close(Intent intent)
        {

        }

        public void StopScrobbling(Intent intent)
        {
            ScrobblePrefs.Clear();
        }

        public static void InitService(Context context)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_INIT);
            context.StartService(serviceIntent);
        }

        public static void SendScrobble(Intent intent, Context context)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_SCROBBLE);
            serviceIntent.PutExtras(intent.Extras);
            context.StartService(serviceIntent);
        }

        public static void StopScrobble(Context context, string artist = null, string album = null, string track = null)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_STOP_SCROBBLE);
            context.StartService(serviceIntent);
        }

        public override void OnDestroy()
        {
            ScrobblePrefs.Clear();
            base.OnDestroy();
        }
    }
}