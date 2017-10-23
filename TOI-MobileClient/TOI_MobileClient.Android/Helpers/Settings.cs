using Plugin.Settings;
using Plugin.Settings.Abstractions;
using TOI_MobileClient.ViewModels;

namespace TOI_MobileClient.Droid.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>        
    public static class Settings
    {
        private readonly string BLUETOOTH = "Bluetooth Enabled";
        private readonly string GPS = "GPS Enabled";

        private CrossSettings _appSettings;

        static Settings()
        {
            _appSettings = CrossSettings.Current;
        }

        public static bool BluetoothEnabled
        {
            get => AppSettings.GetValueOrDefault(BLUETOOTH, true);
            set => AppSettings.AddOrUpdateValue(BLUETOOTH, value);
        }

        public static bool GpsEnabled
        {
            get => AppSettings.GetValueOrDefault(GPS, true);
            set => AppSettings.AddOrUpdateValue(GPS, value);
        }


        public static List<SettingsPageViewModel> GetViewModels()
        {
            var viewModel = new List<SettingViewModel> {
                new RadioSettingViewModel(new RadioSetting(lang.ScanFrequency, new List<string>
                {
                    lang.Often,
                    lang.Normal,
                    lang.Rarely,
                    lang.Never
                }, 1)),
                new BooleanSettingViewModel(new BooleanSetting(lang.GPS)),
                new BooleanSettingViewModel(new BooleanSetting(lang.Bluetooth)),
                new BooleanSettingViewModel(new BooleanSetting(lang.Wifi)),
                new BooleanSettingViewModel(new BooleanSetting(lang.NFC)),
            };

            return viewModel;
        }
    }
}