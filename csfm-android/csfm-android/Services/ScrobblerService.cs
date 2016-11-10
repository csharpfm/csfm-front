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
                    //case ACTION_INIT:
                    //    Init(intent);
                    //    return StartCommandResult.Sticky;
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

        //private void Init(Intent intent)
        //{
        //    MusicPlayingReceiver receiver = new MusicPlayingReceiver();
        //    IntentFilter iF = new IntentFilter().AddActions(Configuration.Music.MUSIC_ACTIONS);
            
        //    RegisterReceiver(receiver, iF);

        //    AppNotificationManager.SendNotification(null, this, this.ApplicationContext);
        //}

        private void Scrobble(Intent intent)
        {
            string artist = intent.GetStringExtra(MediaStore.Audio.AudioColumns.Artist);
            string album = intent.GetStringExtra(MediaStore.Audio.AudioColumns.Album);
            string track = intent.GetStringExtra(MediaStore.Audio.AudioColumns.Track);
            string albumArt = MusicLibrary.GetAlbumArt(artist, album, CSFMApplication.Context)?.FirstOrDefault();
            AppNotificationManager.SendNotification(artist, album, track, this, this.ApplicationContext);

            ScrobblePrefs.Save(artist, album, track);
        }

        private void Close(Intent intent)
        {

        }

        public void StopScrobbling(Intent intent)
        {
            AppNotificationManager.SendNotification(intent.GetArtist(), intent.GetAlbum(), intent.GetTrack(), this, this.ApplicationContext);
            ScrobblePrefs.Clear();
        }

        public static void InitService(Context context)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_INIT);
            context.StartService(serviceIntent);
        }

        public static void SendScrobble(string artist, string album, string track, Context context)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_SCROBBLE);
            Bundle extras = new Bundle();
            extras.AddSong(artist, album, track);
            serviceIntent.PutExtras(extras);
            context.StartService(serviceIntent);
        }

        public static void StopScrobble(Context context, string artist = null, string album = null, string track = null)
        {
            Intent serviceIntent = new Intent(context, typeof(ScrobblerService));
            serviceIntent.SetAction(ACTION_STOP_SCROBBLE);
            Bundle extras = new Bundle();
            extras.AddSong(artist, album, track);
            context.StartService(serviceIntent);
        }

        public override void OnDestroy()
        {
            ScrobblePrefs.Clear();
            base.OnDestroy();
        }
    }
}