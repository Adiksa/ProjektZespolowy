using Firebase.Messaging;
using Android.Graphics;
using Android.App;
using Android.Content;
using System;
using Android.Support.V4.App;
namespace ProjektZespolowy
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        private int count = 0;

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            SendNotificatios(message.GetNotification().Body.ToString(), message.GetNotification().Title.ToString());

        }
        public void SendNotificatios(string body, string Header)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

            var builder = new NotificationCompat.Builder(this, GetString(Resource.String.channel_name))
                .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent(pendingIntent) // Start up this activity when the user clicks the intent.
                .SetContentTitle(Header)
                .SetNumber(count)
                .SetContentText(body)
                .SetSmallIcon(Resource.Mipmap.ic_launcher); // This is the icon to display
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(1, builder.Build());

            count++;
        }
    }
}