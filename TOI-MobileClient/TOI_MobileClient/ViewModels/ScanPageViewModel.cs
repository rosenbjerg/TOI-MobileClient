using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Rosenbjerg.DepMan;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    class ScanPageViewModel : PageViewModelBase
    {
        public override string PageTitle => "Scan for tags";
        public ICommand SyncCommand { get; private set; }

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
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
            get { return !_loading; }
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
            get { return _nearbyTags; }
            set
            {
                if (value == _nearbyTags)
                    return;
                _nearbyTags = value;
                OnPropertyChanged();
            }
        }

        public ScanPageViewModel()
        {
            SyncCommand = new Command(ScanForBLE);
        }

        private async void ScanForBLE()
        {
            Loading = true;

            var scanner = DependencyManager.Get<BleScannerBase>();
            Console.WriteLine("Scan started");
            var devs = await scanner.ScanDevices();
            Console.WriteLine("Scan completed");

            var tvms = new List<TagViewModel>();
            var devsList = devs.ToList();
            devsList.ForEach(d =>
            {
                //Todo: Request the server for information here!

                //tvms.Add(new Tag)
            });
            NearbyTags = tvms;
        }
    }
}
