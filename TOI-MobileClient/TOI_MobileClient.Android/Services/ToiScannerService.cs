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
        public int ScanningNotificationServiceId { get; } = 6969;
        public int ToiFoundNotificationServiceId { get; } = 9696;
        public ScannerServiceBinder Binder { get; private set; }

        public CancellationTokenSource ScanLoopToken { get; private set; }
        public Task ScanLoopTask { get; private set; }
        private readonly BleScannerBase _bleScanner = DependencyManager.Get<BleScannerBase>();
        private readonly GpsScannerBase _gpsScanner = DependencyManager.Get<GpsScannerBase>();
        private readonly NfcScannerBase _nfcScanner = DependencyManager.Get<NfcScannerBase>();
        private readonly WiFiScannerBase _wifiScanner = DependencyManager.Get<WiFiScannerBase>();

        public ToiScannerService()
        {
            _bleScanner.ResultFound += OnTagFound;
            _wifiScanner.ResultFound += OnTagFound;
            _nfcScanner.ResultFound += OnTagFound;
            _gpsScanner.ResultFound += (sender, args) =>
            {
                var tois = SubscriptionManager.Instance.GetToisByLocation(args.Location).ToList();
                if (!tois.Any())
                    return;
                ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));

                NewToiFoundNotification();
            };
        }

        private void OnTagFound(object sender, IScanResultEvent args)
        {
            var tois = SubscriptionManager.Instance.GetTois(args.Id).ToList();
            if (!tois.Any()) return;
            ToisFound?.Invoke(this, new ToisFoundEventArgs(tois));
        }

        private void NewToiFoundNotification()
        {
            var lang = DependencyManager.Get<ILanguage>();
            DependencyManager.Get<NotifierBase>()
                    .UpdateAppNotification(
                        ToiFoundNotificationServiceId, lang.NewToi,
                        lang.NewToiExplanation,
                        Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
        }

        public void StartBackgroundScanning()
        {
            if (ScanLoopTask?.IsCanceled == true) return;

            ScanLoopToken = new CancellationTokenSource();
            ScanLoopTask = Task.Run(ScanLoop, ScanLoopToken.Token);
            SettingsManager.IsScanning = true;
            var lang = DependencyManager.Get<ILanguage>();
            DependencyManager.Get<NotifierBase>()
                .UpdateAppNotification(
                    ScanningNotificationServiceId, lang.Scanning,
                    lang.ScanningExplanation,
                    Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
        }


        public void StopBackgroundScanning()
        {
            if (ScanLoopTask?.IsCanceled ?? true) return;

            ScanLoopToken.Cancel();
            SettingsManager.IsScanning = false;

            var lang = DependencyManager.Get<ILanguage>();
            DependencyManager.Get<NotifierBase>()
                .UpdateAppNotification(
                    ScanningNotificationServiceId, lang.ScanningPaused,
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

        private async Task ScanLoop()
        {
            while (!ScanLoopToken.IsCancellationRequested)
            {
                if (!SubscriptionManager.Instance.Initiated)
                {
                    await Task.Delay(1000);
                    continue;
                }

                SettingsManager.IsScanning = true;
                SubscriptionManager.Instance.RefreshTags();

                var bleTask = _bleScanner.ScanAsync();
                var wifiTask = _wifiScanner.ScanAsync();
                var gpsTask = _gpsScanner.ScanAsync();
                await Task.WhenAll(bleTask, wifiTask, gpsTask);
                await Task.Delay(SettingsManager.ScanDelay());

                if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never)
                {
                    SettingsManager.IsScanning = false;
                    return;
                }
            }
        }

        public event EventHandler<ToisFoundEventArgs> ToisFound;
    }
}