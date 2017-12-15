using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Widget;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Droid.Services;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid
{
    public class AndroidNotifier : NotifierBase
    {
        private readonly Dictionary<int, Bitmap> _bitmaps = new Dictionary<int, Bitmap>();
        private readonly NotificationManager _notificationManager;

        public AndroidNotifier(NotificationManager notificationManager)
        {
            _notificationManager =
                notificationManager ?? throw new NullReferenceException("Notification manager was null!");
        }

        public override void DisplaySnackbar(string text, int dur)
        {
            throw new NotImplementedException();
        }

        public override void DisplayToast(string text, bool longDur = true)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Android.App.Application.Context, text,
                    longDur ? ToastLength.Long : ToastLength.Short).Show();
            });
        }

        public override void CancelNotification(int id)
        {
            _notificationManager.Cancel(id);
        }

        public override void UpdateAppNotification(int bgId, string title, string content, int smallIcon, int largeIcon, bool makeNoice = false)
        {
            // Create a PendingIntent that opens the app when it is tapped
            const int pendingIntentId = 0;
            var pendingIntent =
                PendingIntent.GetActivity(Android.App.Application.Context, pendingIntentId,
                    new Intent(Android.App.Application.Context, typeof(MainActivity)),
                    PendingIntentFlags.UpdateCurrent);
            var pPauseScanIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, pendingIntentId,
                new Intent(NotificationActionHandler.PauseScanningFromBackground),
                PendingIntentFlags.CancelCurrent);
            var pScanIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, pendingIntentId,
                new Intent(NotificationActionHandler.StartScanningFromBackground), PendingIntentFlags.CancelCurrent);
            var pStopServiceIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0,
                new Intent(NotificationActionHandler.StopServiceWhenSwiped), PendingIntentFlags.CancelCurrent);

            if (!_bitmaps.ContainsKey(largeIcon))
            {
                _bitmaps[largeIcon] = Bitmap.CreateScaledBitmap(
                    BitmapFactory.DecodeResource(Android.App.Application.Context.ApplicationContext.Resources,
                        largeIcon), 120,
                    120, false);
            }

            var nb = new NotificationCompat.Builder(Android.App.Application.Context.ApplicationContext)
                .SetTicker("ToI Scanner Service")
                .SetContentTitle(title)
                .SetContentText(content)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(smallIcon)
                .SetLargeIcon(_bitmaps[largeIcon])
                .SetVisibility(1)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(content));

                
            if (SettingsManager.IsScanning && bgId == 6969)
            {
                nb.AddAction(Resource.Drawable.Cross, "Pause Scan", pPauseScanIntent);
                nb.SetOngoing(true);
                nb.SetDeleteIntent(null);
            }
            if(!SettingsManager.IsScanning && bgId == 6969)
            {
                nb.AddAction(Resource.Drawable.Cross, "Start Scan", pScanIntent);
                nb.SetDeleteIntent(pStopServiceIntent);
            }

            if (makeNoice)
                nb.SetDefaults((int) NotificationDefaults.All);

            var not = nb.Build();
            _notificationManager.Notify(bgId, not);
        }
    }
}