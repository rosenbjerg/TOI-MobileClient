using System;
using System.Collections.Generic;
using System.Text;
using TOI_MobileClient.Managers;
using TOI_MobileClient.Models;

namespace TOI_MobileClient.ViewModels
{
    class SettingsPageViewModel : PageViewModelBase
    {
        public override string PageTitle => SettingsManager.Language.Settings;
        public List<SettingViewModel> Settings { get; }

        public SettingsPageViewModel()
        {
            var lang = SettingsManager.Language;
            // TODO: read and write on disk
            Settings = new List<SettingViewModel>
            {
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
        }
    }

    class SettingViewModel : ViewModelBase
    {
        private readonly Setting _setting;
        public string Title => _setting.Title;
        public Setting.SettingType Type => _setting.Type;

        public SettingViewModel(Setting setting)
        {
            _setting = setting;
        }
    }

    class RadioSettingViewModel : SettingViewModel
    {
        private readonly RadioSetting _setting;
        public List<string> Options => _setting.Options;
        public int Selected => _setting.Selected;
        public string SelectedValue => _setting.SelectedValue;

        public RadioSettingViewModel(RadioSetting setting) : base(setting)
        {
            _setting = setting;
        }
    }

    class BooleanSettingViewModel : SettingViewModel
    {
        private readonly BooleanSetting _setting;

        public bool Toggle
        {
            get => _setting.Toggle;
            set
            {
                Console.WriteLine("Setting " + _setting.Title + " to " + value);
                _setting.Toggle = value;
                OnPropertyChanged(nameof(Toggle));
            }
        }

        public BooleanSettingViewModel(BooleanSetting setting) : base(setting)
        {
            _setting = setting;
        }
    }
}