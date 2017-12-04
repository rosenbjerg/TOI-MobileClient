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
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Droid.Services;

namespace TOI_MobileClient.Droid
{
    public class NotificationActionHandler : BroadcastReceiver
    {
        public NotificationActionHandler(Context activity)
        {
            var killAppIntentFilter = new IntentFilter(PauseScanningFromBackground);
            var startScanningIntentFilter = new IntentFilter(StartScanningFromBackground);
            activity.RegisterReceiver(this, killAppIntentFilter);
            activity.RegisterReceiver(this, startScanningIntentFilter);
        }

        public const string PauseScanningFromBackground = "android.toi.PAUSE_SCAN_NOTIFIER";
        public const string StartScanningFromBackground = "android.toi.START_SCAN_NOTIFIER";

        public override async void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine($"Received a broadcast: {intent.Action}");
            if (intent.Action == PauseScanningFromBackground)
            {
                Console.WriteLine("Pause scanning --- Just kidding do nothing");
                (await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync()).StopLoop();
            }

            if (intent.Action == StartScanningFromBackground)
            {
                Console.WriteLine("Start scanning --- Just kidding do nothing");
                (await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync()).StartLoop();
            }
        }
    }
}