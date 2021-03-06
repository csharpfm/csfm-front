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
using csfm_android.Activities;

namespace csfm_android.Notifications
{
    /// <summary>
    /// Class to manage notifications building and sending
    /// </summary>
    public class AppNotificationManager
    {
        private const int NOTIFICATION_ID = 1;
        private static readonly string TITLE = Application.Context.GetString(Resource.String.now_scrobbling);
        private static readonly string TICKER = Configuration.LABEL + " " + Application.Context.GetString(Resource.String.service);

        /// <summary>
        /// Send a notification with the current music track info
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="track"></param>
        /// <param name="service"></param>
        /// <param name="context"></param>
        public static void SendNotification(string artist, string album, string track, Service service, Context context)
        {
            if (artist.IsStringEmpty())
                artist = "Unknwon Artist";
            if (album.IsStringEmpty())
                album = null;
            if (track.IsStringEmpty())
                track = "Unknown Track";

            if (album != null)
                SendNotification(string.Format("{0} - {1} ({2})", artist, track, album), service, context);
            else
                SendNotification(string.Format("{0} - {1}", artist, track), service, context);
        }

        /// <summary>
        /// Send notification with the text to display
        /// </summary>
        /// <param name="trackFormat">Text to display</param>
        /// <param name="service"></param>
        /// <param name="context"></param>
        public static void SendNotification(string trackFormat, Service service, Context context)
        {
            Notification notification = MakeNotification(trackFormat, Resource.Drawable.ic_notifications_mfm, context.Resources.GetColor(Resource.Color.colorPrimary), context);
            SendNotification(notification, service, context);
        }
     
        /// <summary>
        /// Create the notification
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="drawable">Icon</param>
        /// <param name="color">Notification color</param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Notification MakeNotification(string text, int drawable, int color, Context context)
        {
            Intent notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, notificationIntent, 0);

            var builder = new NotificationCompat.Builder(context)
                .SetContentTitle(TITLE)
                .SetTicker(TICKER)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(drawable)
                .SetOngoing(false)
                .SetColor(color);

            if (!text.IsStringEmpty())
                builder.SetContentText(text);

            return builder.Build();
        }   

        /// <summary>
        /// Make a notification action
        /// </summary>
        /// <param name="title">Action title</param>
        /// <param name="actionTag">Action Id</param>
        /// <param name="drawable">Action icon (not used in Android Nougat)</param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static NotificationCompat.Action MakeAction(string title, string actionTag, int drawable, Context context)
        {
            Intent intent = new Intent(context, typeof(ScrobblerService));
            intent.SetAction(actionTag);
            PendingIntent pIntent = PendingIntent.GetService(context, 0, intent, 0);
            return new NotificationCompat.Action.Builder(drawable, title, pIntent).Build();
        }

        /// <summary>
        /// Send the notification
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="service"></param>
        /// <param name="context"></param>
        private static void SendNotification(Notification notification, Service service, Context context)
        {
            Notify(notification, context);
        }

        /// <summary>
        /// Send the notification
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="context"></param>
        private static void Notify(Notification notification, Context context)
        {
            NotificationManager nm = (NotificationManager) context.GetSystemService(Context.NotificationService);
            nm.Notify(NOTIFICATION_ID, notification);
        }

   
    }
}