using System;
using System.Collections.Generic;
using System.Text;
using TOI_MobileClient.Localization;

namespace TOI_MobileClient.Managers
{
    public static class SettingsManager
    {
        public static ILanguage Language { get; set; }

        static SettingsManager()
        {
            Language = new EnglishLanguage();
        }
    }
}
