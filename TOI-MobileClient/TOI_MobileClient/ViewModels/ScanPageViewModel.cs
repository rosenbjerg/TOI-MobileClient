using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using DepMan;
using Newtonsoft.Json;
using TOIClasses;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    public class ScanPageViewModel : PageViewModelBase
    {
        public override string PageTitle => "Scan for tags";
        public ICommand SyncCommand { get; }
        public ICommand ClearCommand { get; }

        private bool _loading;

        public bool Loading
        {
            get => _loading;
            private set
            {
                if (_loading == value)
                    return;
                _loading = value;
                OnPropertyChanged(nameof(Loaded));
                OnPropertyChanged(nameof(SyncColor));
                OnPropertyChanged();
            }
        }

        public bool Loaded
        {
            get => !_loading;
            private set
            {
                if (_loading != value)
                    return;
                Loading = !value;
            }
        }

        public ObservableCollection<ToiViewModel> ToiCollection { get; } = new ObservableCollection<ToiViewModel>();

        public bool FoundTois => ToiCollection.Count > 0;
        public bool NoTags => ToiCollection.Count == 0;
        public Color SyncColor => Loading ? Styling.DisabledIconColor : Styling.EnabledIconColor;

        public bool SyncBtnVisibility => SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never;
        public bool ClearBtnVisibility => SettingsManager.ScanFrequencyValue != SettingsManager.Language.Never;

        public bool PullToRefresh =>
            SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never;

        private IBackgroundScanner _scanner;

        public ScanPageViewModel()
        {
            SyncCommand = new Command(Sync);
            ClearCommand = new Command(() =>
            {
                // Empty ToiCache from ToiScannerService
                SubscriptionManager.Instance.ClearShown();
                _shownTois.Clear();
                ToiCollection.Clear();
                OnPropertyChanged(nameof(FoundTois));
                OnPropertyChanged(nameof(NoTags));
            });
        }

        private readonly HashSet<ToiModel> _shownTois = new HashSet<ToiModel>();

        private void ToisFound(object sender, ToisFoundEventArgs e)
        {
            foreach (var t in e.Tois)
            {
                if (_shownTois.Contains(t)) continue;

                _shownTois.Add(t);
                ToiCollection.Add(new ToiViewModel(t));
            }
            OnPropertyChanged(nameof(FoundTois));
            OnPropertyChanged(nameof(NoTags));
        }

        private void Sync()
        {
            if (Loading)
                return;

            ToiCollection.Clear();
            Loading = true;

            var task = _scanner.StartScan();
            task.Wait();
            Loading = false;
        }

        public override async void OnViewAppearing()
        {
            OnPropertyChanged(null);

            if (_scanner == null)
                _scanner = await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync();

            _scanner.ToisFound += ToisFound;
        }

        public override async void OnViewDisappearing()
        {
            if (_scanner == null)
                _scanner = await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync();

            _scanner.ToisFound += ToisFound;
        }
    }
}