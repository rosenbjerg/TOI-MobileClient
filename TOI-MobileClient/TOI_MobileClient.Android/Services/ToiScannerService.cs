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

        public List<ToiModel> ToiCache { get; } = new List<ToiModel>();
        //public Dictionary<ToiModel, DateTime> ToiTtl { get; } = new Dictionary<ToiModel, DateTime>();

//        public bool ShouldShow(ToiModel toi)
//        {
//            if (!ToiTtl.TryGetValue(toi, out var time))
//            {
//                ToiTtl[toi] = DateTime.UtcNow;   
//                return true;
//            }
//
//            if (time.AddMinutes(15) > DateTime.UtcNow)
//            {
//                return false;
//            }
//
//            ToiTtl[toi] = DateTime.UtcNow;
//            return true;
//        }

        public ToiScannerService()
        {
//            TagsFound += async delegate(object sender, TagsFoundsEventArgs args)
//            {
//                await Task.Delay(1000);
//                if (args.Handled) return;
//
//                var lang = DependencyManager.Get<ILanguage>();
//
//                if (args.Tags.Count == 0)
//                {
//                    DependencyManager.Get<NotifierBase>().UpdateAppNotification(ServiceId, lang.Scanning,
//                        lang.ScanningExplanation,
//                        Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
//                }
//                else
//                {
//                    DependencyManager.Get<NotifierBase>().UpdateAppNotification(ServiceId, lang.NewToi,
//                        lang.NewToiExplanation,
//                        Resource.Drawable.TagFoundIcon, Resource.Drawable.Icon, true);
//                }
//            };


            DependencyManager.Get<BleScannerBase>().BleDeviceFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetTois(args.Device.Address)
                    .Where(t => !ToiCache.Contains(t))
                    .ToList();
                if (!tois.Any()) return;

                ToiCache.AddRange(tois);
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
            };

            DependencyManager.Get<WiFiScannerBase>().WifiApFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetTois(args.Bssid)
                    .Where(t => !ToiCache.Contains(t))
                    .ToList();
                if (!tois.Any()) return;

                ToiCache.AddRange(tois);
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
            };

            DependencyManager.Get<NfcScannerBase>().NfcTagFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetTois(args.TagId)
                    .Where(t => !ToiCache.Contains(t))
                    .ToList();
                if (!tois.Any()) return;

                ToiCache.AddRange(tois);
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
            };

            DependencyManager.Get<GpsScannerBase>().LocationFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetToisByLocation(args.Location)
                    .Where(t => !ToiCache.Contains(t))
                    .ToList();
                if (!tois.Any()) return;

                ToiCache.AddRange(tois);
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