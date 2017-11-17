using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Widget;
using TOI_MobileClient.Dependencies;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid
{
    public class AndroidNotifier : NotifierBase
    {
        private readonly Dictionary<int, Bitmap> _bitmaps = new Dictionary<int, Bitmap>();
        private readonly NotificationManager _notificationManager;

        public AndroidNotifier(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager ?? throw new NullReferenceException("Notification manager was null!");
        }

        public override void DisplaySnackbar(string text, bool longDur = true)
        {
            var activity = (Activity)Forms.Context;
            var view = activity.FindViewById(Android.Resource.Id.Content);

            Device.BeginInvokeOnMainThread(() =>
            {
                Snackbar.Make(view, text, longDur ? 10000 : 3000).Show();
            });
        }

        public override void DisplayToast(string text, bool longDur = true)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Forms.Context, text, longDur ? ToastLength.Long : ToastLength.Short).Show();
            });
            
        }

        public override void DisplayNewToi(int bgId, string title, string content, int smallIcon, int largeIcon, bool makeNoice = false)
        {
            // Create a PendingIntent that opens the app when it is tapped
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(Forms.Context, pendingIntentId, new Intent(Forms.Context, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);

            if (!_bitmaps.ContainsKey(largeIcon))
            {
                _bitmaps[largeIcon] = Bitmap.CreateScaledBitmap(
                    BitmapFactory.DecodeResource(Forms.Context.ApplicationContext.Resources, largeIcon), 120,
                    120, false);
            }

            var nb = new NotificationCompat.Builder(Forms.Context.ApplicationContext)
                .SetTicker("New thing of interest")
                .SetContentTitle(title)
                .SetContentText(content)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(smallIcon)
                .SetLargeIcon(_bitmaps[largeIcon])
                .SetVisibility(1)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(content))
            ;
            if (makeNoice)
                nb.SetDefaults((int) NotificationDefaults.All);


            var not = nb.Build();
            _notificationManager.Notify(bgId, not);
        }
    }
}