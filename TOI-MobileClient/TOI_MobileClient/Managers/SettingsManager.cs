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

        public static int ScanFrequency
        {
            get => AppSettings.GetValueOrDefault(nameof(ScanFrequency), 1);
            set => AppSettings.AddOrUpdateValue(nameof(ScanFrequency), value);
        }

        public static List<string> ScanFrequencyOptions => new List<string>
        {
            Language.Often,
            Language.Normal,
            Language.Rarely,
            Language.Never
        };

        public static string ScanFrequencyValue => ScanFrequencyOptions[ScanFrequency];

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
            if (ScanFrequencyValue == Language.Often) return 5000;
            if (ScanFrequencyValue == Language.Normal) return 15000;
            return ScanFrequencyValue == Language.Rarely ? 60000 : 10000;
        }

        static SettingsManager()
        {
            Language = new EnglishLanguage();
        }
    }
}