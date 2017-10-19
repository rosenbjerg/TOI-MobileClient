using System;
using System.Collections.Generic;
using System.Text;
using TOI_MobileClient.Models;

namespace TOI_MobileClient.ViewModels
{
    class SettingsPageViewModel : PageViewModelBase
    {
        public override string PageTitle => "Settings";
        public List<SettingViewModel> Settings { get; }

        public SettingsPageViewModel()
        {
            // TODO: read and write on disk
            Settings = new List<SettingViewModel>
            {
                new RadioSettingViewModel(new RadioSetting("Scan Frequency", new List<string>
                {
                    "Often",
                    "Normal",
                    "Rarely",
                    "Never"
                }, 1)),
                new BooleanSettingViewModel(new BooleanSetting("GPS")),
                new BooleanSettingViewModel(new BooleanSetting("Bluetooth")),
                new BooleanSettingViewModel(new BooleanSetting("Wi-Fi")),
                new BooleanSettingViewModel(new BooleanSetting("NFC")),
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