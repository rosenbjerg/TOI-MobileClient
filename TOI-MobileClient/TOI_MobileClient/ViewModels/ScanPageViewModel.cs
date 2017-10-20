using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Rosenbjerg.DepMan;
using TOIClasses;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    class ScanPageViewModel : PageViewModelBase
    {
        public override string PageTitle => "Scan for tags";
        public ICommand SyncCommand { get; }

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
                _loading = !value;
                OnPropertyChanged(nameof(Loading));
                OnPropertyChanged();
            }
        }

        private List<TagViewModel> _nearbyTags;

        public List<TagViewModel> NearbyTags
        {
            get => _nearbyTags;
            set
            {
                if (value == _nearbyTags)
                    return;
                _nearbyTags = value;
                OnPropertyChanged(nameof(FoundTags));
                OnPropertyChanged(nameof(NoTags));
                OnPropertyChanged();
            }
        }

        public bool FoundTags => _nearbyTags.Count > 0;

        public bool NoTags => _nearbyTags.Count == 0;

        public ScanPageViewModel()
        {
            SyncCommand = new Command(ScanForBle);
            NearbyTags = new List<TagViewModel>();
        }

        private async void ScanForBle()
        {
            Loading = true;

            var scanner = DependencyManager.Get<BleScannerBase>();
            Console.WriteLine("Scan started");
            var devs = await scanner.ScanDevices(null);
            Console.WriteLine("Scan completed");

            var tvms = new List<TagViewModel>();
            var devsList = devs.ToList();
            var rc = DependencyManager.Get<RestClient>();

            devsList.ForEach(async d =>
            {
                var ti = await rc.Get<TagInfo>("localhost:5000/tags/" + d.Address.ToString("N"));
                tvms.Add(new TagViewModel(ti));
            });
            NearbyTags = tvms;

            Loaded = true;
        }
    }
}
