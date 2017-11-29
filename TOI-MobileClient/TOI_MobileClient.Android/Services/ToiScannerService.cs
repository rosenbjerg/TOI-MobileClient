using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Locations;
using Android.Support.V4.App;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using Xamarin.Forms;


namespace TOI_MobileClient.Droid.Services
{
    [Service(Exported = false, Label = "ToiScannerService")]
    public class ToiScannerService : Service, IBackgroundScanner
    {
        public int ServiceId { get; } = 6969;
        public ScannerServiceBinder Binder { get; private set; }
        public HashSet<string> Filter = new HashSet<string>();
        public CancellationTokenSource ScanLoopToken { get; } = new CancellationTokenSource();
        public Task ScanLoopTask { get; private set; }

        public ToiScannerService()
        {
            TagsFound += async delegate(object sender, TagsFoundsEventArgs args)
            {
                await Task.Delay(1000);
                if (args.Handled) return;

                var lang = DependencyManager.Get<ILanguage>();

                if (args.Tags.Count == 0)
                {
                    DependencyManager.Get<NotifierBase>().UpdateAppNotification(ServiceId, lang.Scanning,
                        lang.ScanningExplanation,
                        Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
                }
                else
                {
                    DependencyManager.Get<NotifierBase>().UpdateAppNotification(ServiceId, lang.NewToi,
                        lang.NewToiExplanation,
                        Resource.Drawable.TagFoundIcon, Resource.Drawable.Icon, true);
                }
            };

            DependencyManager.Get<BleScannerBase>().DeviceFound += (sender, args) =>
            {
                TagFound?.Invoke(sender, new TagFoundEventArgs(args.Device.Address));
                Console.WriteLine($"Found BleDevice: {args.Device.Address}");
            };

            StartLoop();
        }

        public void StartLoop()
        {
            if (ScanLoopTask?.IsCanceled ?? false)
            {
                return;
            }

            ScanLoopTask = Task.Factory.StartNew(ScanLoop, ScanLoopToken.Token);
        }

        public void StopLoop()
        {
            if (ScanLoopTask?.IsCanceled ?? true)
            {
                return;
            }

            ScanLoopToken.Cancel();
        }

        public override IBinder OnBind(Intent intent)
        {
            Binder = new ScannerServiceBinder(this);
            return Binder;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var lang = DependencyManager.Get<ILanguage>();
            DependencyManager.Get<NotifierBase>().UpdateAppNotification(ServiceId, lang.Scanning,
                lang.ScanningExplanation,
                Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
            DependencyManager.Get<NfcScannerBase>().NfcTagFound += OnNfcTagFound;
            return StartCommandResult.Sticky;
        }

        private void OnNfcTagFound(object sender, NfcEventArgs nfcEventArgs)
        {
            if (!SettingsManager.NfcEnabled) return;
            TagFound?.Invoke(this, new TagFoundEventArgs(nfcEventArgs.TagId.ToUpper()));
            Console.WriteLine("NFC tag found: " + nfcEventArgs.TagId);
        }

        public async Task ScanForToi(HashSet<string> filter, ScanConfiguration configuration = null)
        {
            // TODO make gps scanning async, to avoid lag in loading animation
            var gps = ScanGps();
            var wifi = await DependencyManager.Get<WiFiScannerBase>().ScanWifi();
            if (configuration == null) configuration = ScanConfiguration.DefaultScanConfiguration;

            var ble = await ScanBle(filter, configuration.UseBle);
            var tags = ble.Select(b => b.Address).Where(filter.Contains).ToList();
            //hvordan skal vi tilføje nfc tags her?
            TagsFound?.Invoke(this, new TagsFoundsEventArgs(tags));
        }

        private static int GetDelay()
        {
            if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Often) return 5000;
            if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Normal) return 15000;
            return SettingsManager.ScanFrequencyValue == SettingsManager.Language.Rarely ? 60000 : 10000;
        }

        private static readonly HashSet<string> BleFilter = new HashSet<string>
        {
            "CC1454015282".TrimStart('0').ToUpper(),
            "FAC4D1038D3D".TrimStart('0').ToUpper(),
            "CBFFB96CA47D".TrimStart('0').ToUpper(),
            "F4B415054205".TrimStart('0').ToUpper()
        };

        private static async Task ScanLoop()
        {
            while (true)
            {
                if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never)
                {
                    await Task.Delay(10000);
                    continue;
                }

                var ble = DependencyManager.Get<BleScannerBase>().ScanDevices(BleFilter);
                var gps = DependencyManager.Get<GpsScannerBase>().GetLocationAsync();
                var wifi = DependencyManager.Get<WiFiScannerBase>().ScanWifi();

                Console.WriteLine($"BLE: {(await ble).Count} devices");
                Console.WriteLine($"GPS: {await gps}");
                Console.WriteLine($"WIFI: {(await wifi).Count()}");
                await Task.Delay(GetDelay());
            }
        }

        public event EventHandler<TagsFoundsEventArgs> TagsFound;
        public event EventHandler<TagFoundEventArgs> TagFound;

        public async Task<IReadOnlyList<BleDevice>> ScanBle(HashSet<string> filter, bool useBle)
        {
            if (!useBle) return new List<BleDevice>();
            var scanner = DependencyManager.Get<BleScannerBase>();
            return await scanner.ScanDevices(filter, 5000);
        }

        public async Task<Location> ScanGps()
        {
            var scanner = DependencyManager.Get<GpsScannerBase>();
            return await scanner.GetLocationAsync();
        }
    }
}