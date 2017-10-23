using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using TOI_MobileClient.Models;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    class SettingsPageViewModel : PageViewModelBase
    {
        public override string PageTitle => SettingsManager.Language.Settings;
        public List<SettingViewModel> Settings { get; }


        public SettingsPageViewModel()
        {
            Settings = new List<SettingViewModel>();
            foreach (var setting in SettingsManager.Settings)
            {
                switch (setting.Type)
                {
                    case Setting.SettingType.Boolean:
                        Settings.Add(new BooleanSettingViewModel((BooleanSetting) setting));
                        break;
                    case Setting.SettingType.Radio:
                        Settings.Add(new RadioSettingViewModel((RadioSetting) setting));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void UpdateCapabilities()
        {
            Console.WriteLine("Checking Capabilities");

//          TODO: no work
//          var capabilities = SettingsManager.Capabilities.ToDictionary(type => type,
//              type => DependencyManager.IsRegistered(type) && DependencyManager.Get(type).IsEnabled);


            var capabilities = new Dictionary<Type, bool>
            {
                {
                    typeof(BleScannerBase),
                    DependencyManager.IsRegistered<BleScannerBase>() &&
                    DependencyManager.Get<BleScannerBase>().IsEnabled
                },
                {
                    typeof(WiFiScannerBase),
                    DependencyManager.IsRegistered<WiFiScannerBase>() &&
                    DependencyManager.Get<WiFiScannerBase>().IsEnabled
                },
                {
                    typeof(NfcScannerBase),
                    DependencyManager.IsRegistered<NfcScannerBase>() &&
                    DependencyManager.Get<NfcScannerBase>().IsEnabled
                },
                {
                    typeof(GpsLocatorBase),
                    DependencyManager.IsRegistered<GpsLocatorBase>() &&
                    DependencyManager.Get<GpsLocatorBase>().IsEnabled
                },
            };

            foreach (var settingViewModel in Settings)
            {
                if (settingViewModel.Type != Setting.SettingType.Boolean) continue;

                var boolViewModel = (BooleanSettingViewModel) settingViewModel;
                if (capabilities.ContainsKey(boolViewModel.Capability))
                {
                    boolViewModel.IsEnabled = capabilities[boolViewModel.Capability];
                }
            }
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

        public int Selected
        {
            get => _setting.Selected;
            set
            {
                _setting.Selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public string SelectedValue => _setting.SelectedValue;

        public RadioSettingViewModel(RadioSetting setting) : base(setting)
        {
            _setting = setting;
        }
    }

    class BooleanSettingViewModel : SettingViewModel
    {
        private readonly BooleanSetting _setting;
        private bool _isEnabled;
        public Type Capability => _setting.Capability;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool Toggle
        {
            get => _setting.Toggle;
            set
            {
                if (!_isEnabled)
                {
                    // TODO: Recheck with DependencyManager (variable type from _setting.Capability)

                    if (value) // toggle off
                    {
                        _setting.Toggle = false;
                    }
                    else
                    {
                        // TODO: Language 
                        DependencyManager.Get<NotifierBase>()
                            .DisplayToast($"Please enable {_setting.Title} and reload page.", false);
                    }

                    OnPropertyChanged(nameof(Toggle));
                    return;
                }

                _setting.Toggle = value;
                OnPropertyChanged(nameof(Toggle));
            }
        }

        public BooleanSettingViewModel(BooleanSetting setting) : base(setting)
        {
            _setting = setting;
            _isEnabled = false;
            _setting.Toggle = _isEnabled;
        }
    }
}