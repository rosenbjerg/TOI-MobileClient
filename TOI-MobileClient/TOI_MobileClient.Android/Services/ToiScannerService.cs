using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Locations;
using Android.Support.V4.App;
using DepMan;
using TOI_MobileClient.Dependencies;
using Xamarin.Forms;


namespace TOI_MobileClient.Droid.Services
{
    [Service(Exported =  false, Label = "ToiScannerService")]

    public class ToiScannerService : Service, IBackgroundScanner
    {
        public int ServiceId { get; } = 6969;
        public ScannerServiceBinder Binder { get; private set; }
        public HashSet<string> Filter = new HashSet<string>();

        public ToiScannerService()
        {
            TagsFound += async delegate(object sender, TagsFoundsEventArgs args)
            {
                await Task.Delay(1000);
                if (args.Handled)
                    return;

                var lang = DependencyManager.Get<ILanguage>();

                if (args.Tags.Count == 0)
                    DependencyManager.Get<NotifierBase>().UpdateAppNotification(ServiceId, lang.Scanning,
                        lang.ScanningExplanation,
                        Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
                else
                    DependencyManager.Get<NotifierBase>().UpdateAppNotification(ServiceId, lang.NewToi,
                        lang.NewToiExplanation,
                        Resource.Drawable.TagFoundIcon, Resource.Drawable.Icon, true);
            };
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
            //Logger.Instance.WriteToLog("MusicPlayerService", "Service started");
            return StartCommandResult.Sticky;
        }

        private void OnNfcTagFound(object sender, NfcEventArgs nfcEventArgs)
        {
            TagsFound?.Invoke(this, new TagsFoundsEventArgs(new List<string> { nfcEventArgs.TagId }));
            Console.WriteLine("NFC tag found: " + nfcEventArgs.TagId);
        }

        public async Task ScanForToi(HashSet<string> filter, ScanConfiguration configuration = null)
        {
            // TODO make gps scanning async, to avoid lag in loading animation
            var gps =  ScanGps();
            var wifi = await DependencyManager.Get<WiFiScannerBase>().ScanWifi();
            if(configuration == null) configuration = ScanConfiguration.DefaultScanConfiguration;
            
            var ble = await ScanBle(filter, configuration.UseBle);
            var tags = ble.Select(b => b.Address).Where(filter.Contains).ToList();
            //hvordan skal vi tilføje nfc tags her?
            TagsFound?.Invoke(this, new TagsFoundsEventArgs(tags));
        }

        private async void ScanLoop()
        {
            while (true)
            {
                //await ScanForToi(new HashSet<string>());

                //TODO: This should depend on the settings
                await Task.Delay(5000);
            }
        }

        public event EventHandler<TagsFoundsEventArgs> TagsFound;

        public async Task<IReadOnlyList<BleDevice>> ScanBle(HashSet<string> filter, bool useBle)
        {
            if (!useBle) return new List<BleDevice>();
            var scanner = DependencyManager.Get<BleScannerBase>();
            return await scanner.ScanDevices(filter, 5000);
        }

        //public Guid HandleNfcIntent()
        //{
            //var scanner = DependencyManager.Get<NfcScannerBase>();
            //return scanner.HandleNfcIntent(Application); Mangler at finde ud af hvordan man parser Intent
        //}

        //public async Task<IReadOnlyList<Position>> ScanGps(double radius)
        public Location ScanGps()
        {
            //tjek om gps er slået til
            var scanner = DependencyManager.Get<GpsLocatorBase>();
            var position = scanner.GetLocation();

            return position;
        }
    }
}
