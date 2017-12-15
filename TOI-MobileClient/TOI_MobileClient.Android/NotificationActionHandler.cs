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
            var stopServiceIntentFilter = new IntentFilter(StopServiceWhenSwiped);
            activity.RegisterReceiver(this, killAppIntentFilter);
            activity.RegisterReceiver(this, startScanningIntentFilter);
            activity.RegisterReceiver(this, stopServiceIntentFilter);
        }
        public bool IsRunning { get; set; }
        public const string PauseScanningFromBackground = "android.toi.PAUSE_SCAN_NOTIFIER";
        public const string StartScanningFromBackground = "android.toi.START_SCAN_NOTIFIER";
        public const string StopServiceWhenSwiped = "android.toi.SERVICE_STOP";

        public override async void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine($"Received a broadcast: {intent.Action}");
            switch (intent.Action)
            {
                case StopServiceWhenSwiped:
                    //her kan servicen nakkes.
                    break;
                case PauseScanningFromBackground:
                    (await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync()).StopBackgroundScanning();
                    break;
                case StartScanningFromBackground:
                    (await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync()).StartBackgroundScanning();
                    break;
                
            }
        }
    }
}