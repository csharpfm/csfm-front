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
using Android.Support.V7.App;
using csfm_android.Services;
using Java.Lang;
using csfm_android.Utils;

namespace csfm_android.Notifications
{
    public class AppNotificationManager
    {
        private const int NOTIFICATION_ID = 1;
        private const string TITLE = "Now Scrobbling";
        private const string TICKER = Configuration.LABEL + " Service";


        public static void SendNotification(string artist, string album, string track, Service service, Context context)
        {
            if (artist.IsStringEmpty())
            {
                artist = "Unknwon Artist";
            }
            if (album.IsStringEmpty())
            {
                album = "Unknown Album";
            }
            if (track.IsStringEmpty())
            {
                track = "Unknown Album";
            }
            SendNotification(string.Format("{0} - {1} ({2})", artist, track, album), service, context);
        }

        public static void SendNotification(string trackFormat, Service service, Context context)
        {
            Notification notification = MakeNotification(trackFormat, Resource.Drawable.Icon, context.Resources.GetColor(Resource.Color.colorPrimary), context);
            SendNotification(notification, service, context);
        }
     
        private static Notification MakeNotification(string text, int drawable, int color, Context context)
        {
            Intent notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, notificationIntent, 0);

            NotificationCompat.Action closeAction = MakeAction("Close", "Close", Resource.Drawable.ic_close_white_24dp, context);

            var builder = new NotificationCompat.Builder(context)
                .SetContentTitle(TITLE)
                .SetTicker(TICKER)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(drawable)
                .SetOngoing(false)
                .SetColor(color)
                .AddAction(closeAction);

            if (!text.IsStringEmpty())
                builder.SetContentText(text);

            return builder.Build();
        }   

        private static NotificationCompat.Action MakeAction(string title, string actionTag, int drawable, Context context)
        {
            Intent intent = new Intent(context, typeof(ScrobblerService));
            intent.SetAction(actionTag);
            PendingIntent pIntent = PendingIntent.GetService(context, 0, intent, 0);
            return new NotificationCompat.Action.Builder(drawable, title, pIntent).Build();
        }

        private static void SendNotification(Notification notification, Service service, Context context)
        {
            Notify(notification, context);
            //service.StartForeground(NOTIFICATION_ID, notification);
            //Intent updateIntent = new Intent("Notification");
            //service.SendBroadcast(updateIntent);
        }

        private static void Notify(Notification notification, Context context)
        {
            NotificationManager nm = (NotificationManager) context.GetSystemService(Context.NotificationService);
            nm.Notify(NOTIFICATION_ID, notification);
        }

   
    }
}