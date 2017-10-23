using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using TOI_MobileClient.Localization;

namespace TOI_MobileClient.Managers
{
    public static class SettingsManager
    {
        public static ILanguage Language { get; set; }
        public static ISettings AppSettings => CrossSettings.Current;
        public static List<string> ScanFrequencyOptions => new List<string>
        {
            Language.Often,
            Language.Normal,
            Language.Rarely,
            Language.Never
        };

        public static string Url => "http://192.168.0.105:7474/tags/";
        static SettingsManager()
        {
            Language = new EnglishLanguage();
        }
    }
}
