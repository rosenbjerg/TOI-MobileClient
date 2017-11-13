using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid.Services
{
    [Service(Exported =  false, Label = "ToiScannerService")]

    public class ToiScannerService : Service, IBackgroundScanner
    {
        public int ServiceId { get; } = 6969;
        public ScannerServiceBinder Binder { get; private set; }

        public override IBinder OnBind(Intent intent)
        {
            Binder = new ScannerServiceBinder(this);
            return Binder;
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var lang = DependencyManager.Get<ILanguage>();
            DependencyManager.Get<NotifierBase>().DisplayNewToi(ServiceId, lang.Scanning,
                lang.ScanningExplanation, 
                Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon);
            //Logger.Instance.WriteToLog("MusicPlayerService", "Service started");
            return StartCommandResult.Sticky;
        }

        public async void ScanForToi(HashSet<Guid> filter, ScanConfiguration configuration = null)
        {
            if(configuration == null) configuration = ScanConfiguration.DefaultScanConfiguration;
            
            var ble = await ScanBle(filter, configuration.UseBle);
            var tags = ble.Select(b => b.Address).Where(filter.Contains).ToList();

            if (TagsFound != null)
                TagsFound.Invoke(this, new TagsFoundsEventArgs(tags));
            else
            {
                var lang = DependencyManager.Get<ILanguage>();
                if(tags.Count == 0)
                    DependencyManager.Get<NotifierBase>().DisplayNewToi(ServiceId, lang.Scanning,
                        lang.ScanningExplanation,
                        Resource.Drawable.TagSyncIcon, Resource.Drawable.Icon, true);
                else
                    DependencyManager.Get<NotifierBase>().DisplayNewToi(ServiceId, lang.NewToi,
                        lang.NewToiExplanation,
                        Resource.Drawable.TagFoundIcon, Resource.Drawable.Icon, true);
            }
        }

        public event EventHandler<TagsFoundsEventArgs> TagsFound;

        public async Task<IReadOnlyList<BleDevice>> ScanBle(HashSet<Guid> filter, bool useBle)
        {
            if (!useBle) return new List<BleDevice>();
            var scanner = DependencyManager.Get<BleScannerBase>();
            return await scanner.ScanDevices(filter);
        }

        //public Task<IReadOnlyList<NfcDevice>> ScanNfc();
        //{
        //    var scanner = DependencyManager.Get<NfcScannerBase>();
        //    //return await scanner
        //}

        //public void ScanGps()
        //{
            
        //}

        //public void ScanWifi()
        //{
            
        //}
    }
}
