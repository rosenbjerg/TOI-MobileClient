using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Localization;
using TOI_MobileClient.Models;
using TOI_MobileClient.ViewModels;

namespace TOI_MobileClient.Managers
{
    public static class SettingsManager
    {
        public static ILanguage Language { get; set; }
        public static ISettings AppSettings => CrossSettings.Current;
        public static string Url => "http://192.168.0.105:7474/tags/";

        public static List<string> ScanFrequencyOptions => new List<string>
        {
            Language.Often,
            Language.Normal,
            Language.Rarely,
            Language.Never
        };

        /// <summary>
        /// Used to access the setting for a given Capability.
        /// </summary>
        public static Dictionary<Type, BooleanSetting> Capabilities { get; }

        public static List<Setting> Settings { get; }

        static SettingsManager()
        {
            Language = new EnglishLanguage();

            var gpsSetting = new BooleanSetting("GPS", Language.Gps) {Capability = typeof(GpsLocatorBase)};
            var bleSetting = new BooleanSetting("BLE", Language.Bluetooth) {Capability = typeof(BleScannerBase)};
            var wifiSetting = new BooleanSetting("Wi-Fi", Language.Wifi) {Capability = typeof(WiFiScannerBase)};
            var nfcSetting = new BooleanSetting("NFC", Language.Nfc) {Capability = typeof(NfcScannerBase)};

            Capabilities = new Dictionary<Type, BooleanSetting>
            {
                {typeof(GpsLocatorBase), gpsSetting},
                {typeof(BleScannerBase), bleSetting },
                {typeof(WiFiScannerBase), wifiSetting },
                {typeof(NfcScannerBase), nfcSetting }
            };

            Settings = new List<Setting>
            {
                new RadioSetting("ScanFrequency", Language.ScanFrequency, ScanFrequencyOptions),
                gpsSetting,
                bleSetting,
                wifiSetting,
                nfcSetting
            };
        }
    }
}