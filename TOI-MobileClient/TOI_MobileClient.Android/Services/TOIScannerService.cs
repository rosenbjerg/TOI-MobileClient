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
using Android.Support.V7.App;
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

        public override IBinder OnBind(Intent intent)
        {
            Binder = new ScannerServiceBinder(this);
            return Binder;
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var notification = _notificationBuilder.BuildNotification();
            StartForeground(ServiceId, notification);
            //Logger.Instance.WriteToLog("MusicPlayerService", "Service started");
            return StartCommandResult.Sticky;
        }

        public async void ScanForToi(HashSet<Guid> filter)
        {
            var ble = await ScanBle(filter);
            var gps = await ScanGps(10);
            var tags = ble.Select(b => b.Address).Where(filter.Contains).ToList();

            TagsFound?.Invoke(this, new TagsFoundsEventArgs(tags));
        }

        public event EventHandler<TagsFoundsEventArgs> TagsFound;

        public async Task<IReadOnlyList<BleDevice>> ScanBle(HashSet<Guid> filter)
        {
            var scanner = DependencyManager.Get<BleScannerBase>();
            return await scanner.ScanDevices(filter);
        }

        //public Guid ScanNfc()
        //{
            //var scanner = DependencyManager.Get<NfcScannerBase>();
            //return scanner.ScanNfc(Application); Mangler at finde ud af hvordan man parser Intent
        //}

        //public async Task<IReadOnlyList<Position>> ScanGps(double radius)
        public async Task<Location> ScanGps(double radius)
        {
            //tjek om gps er slået til
            var scanner = DependencyManager.Get<GpsLocatorBase>();
            var position = scanner.GetLocation();


            return position;
        }

        //public void ScanWifi()
        //{

        //}
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
