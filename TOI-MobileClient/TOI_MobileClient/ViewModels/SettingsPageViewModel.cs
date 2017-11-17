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
    }
    
}