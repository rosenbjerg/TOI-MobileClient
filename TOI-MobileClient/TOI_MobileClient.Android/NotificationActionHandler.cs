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
using TOI_MobileClient.Droid.Services;

namespace TOI_MobileClient.Droid
{
    public class NotificationActionHandler : BroadcastReceiver
    {
        public NotificationActionHandler(Context activity)
        {
            var iFilter = new IntentFilter(KillAppAndService);
            activity.RegisterReceiver(this, iFilter);
        }
        
        public const string KillAppAndService = "android.toi.CLOSE_APP";

        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine($"Received a broadcast: {intent.Action}");
            if (intent.Action != KillAppAndService) return;

            var act = context as Activity;
            Console.WriteLine($"Context as Activity: {act}");
            context.StopService(new Intent(context, typeof(ToiScannerService)));
            act?.FinishActivity(0);
        }
    }
}