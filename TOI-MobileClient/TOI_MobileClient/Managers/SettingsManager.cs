using System;
using System.Collections.Generic;
using System.Linq;
using Android.Database.Sqlite;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using TOI_MobileClient.Localization;
using TOI_MobileClient.ViewModels;

namespace TOI_MobileClient.Managers
{
    public static class SettingsManager
    {
        public static ILanguage Language { get; set; }
        public static ISettings AppSettings => CrossSettings.Current;
        public static string Url => "http://ssh.windelborg.info:7474";

        private const bool Default = true;

        public static void SaveServers(Dictionary<string, SubscribedServer> servers)
        {
            AppSettings.AddOrUpdateValue("subscribed_servers", JsonConvert.SerializeObject(servers.Values));
        }

        public static Dictionary<string, SubscribedServer> ReadServers()
        {
            var values = AppSettings.GetValueOrDefault("subscribed_servers", null);
            if (values == null) return null;

            return JsonConvert.DeserializeObject<List<SubscribedServer>>(values)
                .ToDictionary(e => e.BaseUrl, e => e);
        }

        public static bool IsScanning { get; set; }
        public static bool BleEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(BleEnabled), Default);
            set => AppSettings.AddOrUpdateValue(nameof(BleEnabled), value);
        }

        public static bool WiFiEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(WiFiEnabled), Default);
            set => AppSettings.AddOrUpdateValue(nameof(WiFiEnabled), value);
        }

        public static bool GpsEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(GpsEnabled), Default);
            set => AppSettings.AddOrUpdateValue(nameof(GpsEnabled), value);
        }

        public static bool NfcEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(NfcEnabled), Default);
            set => AppSettings.AddOrUpdateValue(nameof(NfcEnabled), value);
        }

        public static ScanFrequencyEnum ScanFrequency
        {
            get => (ScanFrequencyEnum) AppSettings.GetValueOrDefault(nameof(ScanFrequency), 1);
            set => AppSettings.AddOrUpdateValue(nameof(ScanFrequency), (int) value);
        }

        public static List<string> ScanFrequencyOptions => new List<string>
        {
            Language.Often,
            Language.Normal,
            Language.Rarely,
            Language.Never
        };

        public static string ScanFrequencyValue => ScanFrequencyOptions[(int)ScanFrequency];

        public static string PrepId(string id)
        {
            return id.ToUpperInvariant().Replace(":", "");
        }

        /// <summary>
        /// Hardcoded values for scan delay, depending on the Scan Frequency setting.
        /// </summary>
        /// <returns>Scan delay in ms</returns>
        public static int ScanDelay()
        {
            switch (ScanFrequency)
            {
                case ScanFrequencyEnum.Often:
                    return 5000;
                case ScanFrequencyEnum.Normal:
                    return 15000;
                case ScanFrequencyEnum.Rarely:
                    return 60000;
                default:
                case ScanFrequencyEnum.Never:
                    return 10000;
            }
        }

        static SettingsManager()
        {
            Language = new EnglishLanguage();
        }
    }

    public enum ScanFrequencyEnum
    {
        Often,
        Normal,
        Rarely,
        Never
    }
}