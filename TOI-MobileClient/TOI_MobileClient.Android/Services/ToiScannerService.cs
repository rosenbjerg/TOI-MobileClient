﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        // TODO: Change this shit up to the SettingsManager
        public static HashSet<string> Filter = new HashSet<string>
        {
            SettingsManager.PrepId("84:16:f9:ae:a4:3a"),
            SettingsManager.PrepId("90:A4:DE:E8:29:AC")
        };

        // TODO: Change this shit up to the SettingsManager
        private static readonly HashSet<string> BleFilter = new HashSet<string>
        {
            SettingsManager.PrepId("CC1454015282"),
            SettingsManager.PrepId("FAC4D1038D3D"),
            SettingsManager.PrepId("CBFFB96CA47D"),
            SettingsManager.PrepId("F4B415054205"),
        };

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


            DependencyManager.Get<BleScannerBase>().BleDeviceFound += (sender, args) =>
            {
                TagFound?.Invoke(sender, new TagFoundEventArgs(args.Device.Address));
            };

            DependencyManager.Get<WiFiScannerBase>().WifiApFound += (sender, args) =>
            {
                TagFound?.Invoke(sender, new TagFoundEventArgs(SettingsManager.PrepId(args.Bssid)));
            };

            DependencyManager.Get<NfcScannerBase>().NfcTagFound += (sender, args) =>
            {
                TagFound?.Invoke(this, new TagFoundEventArgs(SettingsManager.PrepId(args.TagId)));
            };

            DependencyManager.Get<GpsScannerBase>().LocationFound += (sender, args) =>
            {
                TagFound?.Invoke(this, new TagFoundEventArgs(args.Location, true));
            };

            StartLoop();
        }

        public void StartLoop()
        {
            if (ScanLoopTask?.IsCanceled ?? false)
            {
                return;
            }

            ScanLoopTask = Task.Run(ScanLoop, ScanLoopToken.Token);
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
            DependencyManager.Get<NotifierBase>().UpdateAppNotification(
                ServiceId,
                lang.Scanning,
                lang.ScanningExplanation,
                Resource.Drawable.TagSyncIcon,
                Resource.Drawable.Icon);

            return StartCommandResult.Sticky;
        }


        public async Task StartScan()
        {
            if (Looping) return;

            //await DependencyManager.Get<GpsScannerBase>().GetLocationAsync();
            //await DependencyManager.Get<BleScannerBase>().ScanBle(BleFilter);
            //await DependencyManager.Get<WiFiScannerBase>().ScanWifi(Filter);
            return;
        }

        private static int GetDelay()
        {
            if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Often) return 5000;
            if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Normal) return 15000;
            return SettingsManager.ScanFrequencyValue == SettingsManager.Language.Rarely ? 60000 : 10000;
        }


        private static async Task ScanLoop()
        {
            while (true)
            {
                if (SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never)
                {
                    Looping = false;
                    await Task.Delay(10000);
                    continue;
                }

                Looping = true;
                await DependencyManager.Get<BleScannerBase>().ScanBle(BleFilter);
                await DependencyManager.Get<WiFiScannerBase>().ScanWifi(Filter);
                var gps = await DependencyManager.Get<GpsScannerBase>().GetLocationAsync();
                Console.WriteLine(gps);
                await Task.Delay(GetDelay());
            }
        }

        public static bool Looping { get; set; }

        public event EventHandler<TagsFoundsEventArgs> TagsFound;
        public event EventHandler<TagFoundEventArgs> TagFound;
    }
}