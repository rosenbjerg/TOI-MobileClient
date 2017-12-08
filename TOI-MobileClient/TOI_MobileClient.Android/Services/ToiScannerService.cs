using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using DepMan;
using TOIClasses;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;


namespace TOI_MobileClient.Droid.Services
{
    [Service(Exported = false, Label = "ToiScannerService")]
    public class ToiScannerService : Service, IBackgroundScanner
    {
        public int ServiceId { get; } = 6969;
        public ScannerServiceBinder Binder { get; private set; }

        public CancellationTokenSource ScanLoopToken { get; private set; }
        public Task ScanLoopTask { get; private set; }
        
        public ToiScannerService()
        {
            DependencyManager.Get<BleScannerBase>().BleDeviceFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetTois(args.Device.Address).ToList();
                if (!tois.Any()) return;
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
            };

            DependencyManager.Get<WiFiScannerBase>().WifiApFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetTois(args.Bssid)
                    .ToList();
                if (!tois.Any()) return;
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
            };

            DependencyManager.Get<NfcScannerBase>().NfcTagFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetTois(args.TagId)
                    .ToList();
                if (!tois.Any()) return;
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
            };

            DependencyManager.Get<GpsScannerBase>().LocationFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetToisByLocation(args.Location)
                    .ToList();
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
            };
            StartLoop();
        }

        public void StartLoop()
        {
            if (ScanLoopTask?.IsCanceled == true) return;

            Console.WriteLine("Starting ScanLoop");
            ScanLoopToken = new CancellationTokenSource();
            ScanLoopTask = Task.Run(ScanLoop, ScanLoopToken.Token);
            Looping = true;
            var lang = DependencyManager.Get<ILanguage>();
            DependencyManager.Get<NotifierBase>()
                .UpdateAppNotification(
                    ServiceId, lang.Scanning,
                    lang.ScanningExplanation,
                    Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
        }

        public void StopLoop()
        {
            if (ScanLoopTask?.IsCanceled ?? true) return;

            Console.WriteLine("Stopping ScanLoop");
            ScanLoopToken.Cancel();
            Looping = false;

            var lang = DependencyManager.Get<ILanguage>();
            DependencyManager.Get<NotifierBase>()
                .UpdateAppNotification(
                    ServiceId, lang.ScanningPaused,
                    lang.NotScanningExplanation,
                    Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
        }

        public override IBinder OnBind(Intent intent)
        {
            Binder = new ScannerServiceBinder(this);
            return Binder;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }


        public async Task StartScan()
        {
            if (Looping) return;

            //await DependencyManager.Get<GpsScannerBase>().GetLocationAsync();
            //await DependencyManager.Get<BleScannerBase>().ScanBle(BleFilter);
            //await DependencyManager.Get<WiFiScannerBase>().ScanAsync(Filter);
            return;
        }


        private async Task ScanLoop()
        {
            while (!ScanLoopToken.IsCancellationRequested)
            {
                if (!SubscriptionManager.Instance.Inited)
                {
                    await Task.Delay(1000);
                    continue;
                }

                if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never)
                {
                    Looping = false;
                    await Task.Delay(SettingsManager.ScanDelay());
                    continue;
                }

                Looping = true;
                SubscriptionManager.Instance.RefreshTags();

                Console.WriteLine("Starting Bluetooth Low Energy scan.");
                await DependencyManager.Get<BleScannerBase>().ScanAsync();
                Console.WriteLine("Starting Wi-Fi scan.");
                await DependencyManager.Get<WiFiScannerBase>().ScanAsync();
//                Console.WriteLine("Starting GPS scan.");
//                await DependencyManager.Get<GpsScannerBase>().GetLocationAsync();
                Console.WriteLine("Finished scanning, delaying...");
                await Task.Delay(SettingsManager.ScanDelay());
            }
        }

        public static bool Looping { get; set; }

        public event EventHandler<ToisFoundEventArgs> ToisFound;
    }
}