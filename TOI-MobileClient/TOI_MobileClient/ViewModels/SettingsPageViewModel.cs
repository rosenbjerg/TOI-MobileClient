using System.Collections.Generic;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.ViewModels
{
    public class SettingsPageViewModel : PageViewModelBase
    {
        public override string PageTitle => SettingsManager.Language.Settings;

        public string BleEnabledTitle => SettingsManager.Language.Bluetooth;

        public bool BleEnabled
        {
            get => SettingsManager.BleEnabled;
            set
            {
                if (!DependencyManager.IsRegistered<BleScannerBase>() ||
                    !DependencyManager.Get<BleScannerBase>().IsEnabled)
                {
                    DependencyManager.Get<NotifierBase>()
                        .DisplayToast(SettingsManager.Language.EnableToast + BleEnabledTitle, false);
                    OnPropertyChanged();
                    return;
                }

                SettingsManager.BleEnabled = value;
                OnPropertyChanged();
            }
        }

        public string WiFiEnabledTitle => SettingsManager.Language.Wifi;

        public bool WiFiEnabled
        {
            get => SettingsManager.WiFiEnabled;
            set
            {
                if (!DependencyManager.IsRegistered<WiFiScannerBase>() ||
                    !DependencyManager.Get<WiFiScannerBase>().IsEnabled)
                {
                    DependencyManager.Get<NotifierBase>()
                        .DisplayToast(SettingsManager.Language.EnableToast + WiFiEnabledTitle, false);
                    OnPropertyChanged();
                    return;
                }

                SettingsManager.WiFiEnabled = value;
                OnPropertyChanged();
            }
        }

        public string GpsEnabledTitle => SettingsManager.Language.Gps;

        public bool GpsEnabled
        {
            get => SettingsManager.GpsEnabled;
            set
            {
                if (!DependencyManager.IsRegistered<GpsLocatorBase>() ||
                    !DependencyManager.Get<GpsLocatorBase>().IsEnabled)
                {
                    DependencyManager.Get<NotifierBase>()
                        .DisplayToast(SettingsManager.Language.EnableToast + GpsEnabledTitle, false);
                    OnPropertyChanged();
                    return;
                }

                SettingsManager.GpsEnabled = value;
                OnPropertyChanged();
            }
        }

        public string NfcEnabledTitle => SettingsManager.Language.Nfc;

        public bool NfcEnabled
        {
            get => SettingsManager.NfcEnabled;
            set
            {
                if (!DependencyManager.IsRegistered<NfcScannerBase>() ||
                    !DependencyManager.Get<NfcScannerBase>().IsEnabled)
                {
                    DependencyManager.Get<NotifierBase>()
                        .DisplayToast(SettingsManager.Language.EnableToast + NfcEnabledTitle, false);
                    OnPropertyChanged();
                    return;
                }

                SettingsManager.WiFiEnabled = value;
                OnPropertyChanged();
            }
        }

        public string ScanFrequencyTitle => SettingsManager.Language.ScanFrequency;

        public int ScanFrequency
        {
            get => SettingsManager.ScanFrequency;
            set
            {
                SettingsManager.ScanFrequency = value;
                OnPropertyChanged();
            }
        }

        public List<string> ScanFrequencyOptions => SettingsManager.ScanFrequencyOptions;

        public SettingsPageViewModel()
        {
            SettingsManager.BleEnabled = DependencyManager.IsRegistered<BleScannerBase>() &&
                                         DependencyManager.Get<BleScannerBase>().IsEnabled;
            SettingsManager.WiFiEnabled = DependencyManager.IsRegistered<WiFiScannerBase>() &&
                                         DependencyManager.Get<WiFiScannerBase>().IsEnabled;
            SettingsManager.GpsEnabled = DependencyManager.IsRegistered<GpsLocatorBase>() &&
                                         DependencyManager.Get<GpsLocatorBase>().IsEnabled;
            SettingsManager.NfcEnabled = DependencyManager.IsRegistered<NfcScannerBase>() &&
                                         DependencyManager.Get<NfcScannerBase>().IsEnabled;
        }
    }
    
}