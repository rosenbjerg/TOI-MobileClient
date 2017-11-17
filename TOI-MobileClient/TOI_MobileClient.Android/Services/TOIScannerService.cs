using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using TOI_MobileClient.Models;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid.Services
{
    [Service(Exported =  false, Label = "ToiScannerService")]

    public class ToiScannerService : Service, IBackgroundScanner
    {
        public int ServiceId { get; } = 6969;
        public ScannerServiceBinder Binder { get; private set; }
        private NotificationBuilder _notificationBuilder = new NotificationBuilder();
        public HashSet<string> Filter = new HashSet<string>();

        public override IBinder OnBind(Intent intent)
        {
            Binder = new ScannerServiceBinder(this);
            return Binder;
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var notification = _notificationBuilder.BuildNotification();
            StartForeground(ServiceId, notification);
            DependencyManager.Get<NfcScannerBase>().NfcTagFound += OnNfcTagFound;
            //Logger.Instance.WriteToLog("MusicPlayerService", "Service started");
            return StartCommandResult.Sticky;
        }

        private void OnNfcTagFound(object sender, NfcEventArgs nfcEventArgs)
        {
            TagsFound?.Invoke(this, new TagsFoundsEventArgs(new List<string> { nfcEventArgs.TagId }));
            Console.WriteLine("NFC tag found: " + nfcEventArgs.TagId);
        }

        public async void ScanForToi(HashSet<string> filter)
        {

            var ble = await ScanBle(filter);
            var gps =  ScanGps();
            var wifi = await DependencyManager.Get<WiFiScannerBase>().ScanWifi();
            var tags = ble.Select(b => b.Address).Where(filter.Contains).ToList();
            //hvordan skal vi tilføje nfc tags her?
            TagsFound?.Invoke(this, new TagsFoundsEventArgs(tags));
        }

        public event EventHandler<TagsFoundsEventArgs> TagsFound;

        public async Task<IReadOnlyList<BleDevice>> ScanBle(HashSet<string> filter)
        {
            var scanner = DependencyManager.Get<BleScannerBase>();
            return await scanner.ScanDevices(filter);
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

    internal class NotificationBuilder
    {
        //Todo This class should probably have a platform-independent implementation
        private readonly Bitmap _largeIcon;

        public NotificationBuilder()
        {
            _largeIcon = Bitmap.CreateScaledBitmap(
                BitmapFactory.DecodeResource(Forms.Context.ApplicationContext.Resources, Resource.Drawable.Icon), 120,
                120, false);
        }
        public Notification BuildNotification()
        {
            return new NotificationCompat.Builder(Forms.Context.ApplicationContext)
                .SetTicker("TOI Scanner")
                .SetContentTitle("TOI Scanner")
                .SetContentText("Scanning for TOIs in the background.")
                .SetSmallIcon(Resource.Drawable.TagSyncIcon)
                .SetLargeIcon(_largeIcon)
                .SetVisibility(1)
                //.SetStyle()
                .Build();
        }
    }
}
