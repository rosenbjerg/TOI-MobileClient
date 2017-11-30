using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using TOIClasses;
using TOI_MobileClient.Dependencies;
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

        public static Dictionary<string, List<ContextViewModel>> Subscriptions { get; set; }

        public static List<ContextViewModel> CurrentContext => Subscriptions[Url];


        public static string PrepId(string id)
        {
            return id.ToUpperInvariant().Replace(":", "");
        }


        static SettingsManager()
        {
            Language = new EnglishLanguage();
            Subscriptions = new Dictionary<string, List<ContextViewModel>>();
        }
    }
}