using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private List<ToiViewModel> _nearbyTags;

        public List<ToiViewModel> NearbyTags
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

        public Color SyncColor => Loading ? Styling.DisabledIconColor : Styling.EnabledIconColor;

        private IBackgroundScanner _scanner;

        public ScanPageViewModel()
        {
            SyncCommand = new Command(ScanForToi);
            NearbyTags = new List<ToiViewModel>();
        }
        

        private async void OnTagsFound(object sender, TagsFoundsEventArgs tagsFoundsEventArgs)
        {
            tagsFoundsEventArgs.Handled = true;
            var rc = DependencyManager.Get<RestClient>();
            try
            {
                var tvms = await rc.GetMany<ToiModel>(SettingsManager.Url + "/toi/fromtags", tagsFoundsEventArgs.Tags);
                if (tvms == null)
                {
                    NearbyTags = new List<ToiViewModel>();
                    DependencyManager.Get<NotifierBase>().DisplayToast("No tags found", false);

                }
                else
                {
                    NearbyTags = tvms.Select(t => new ToiViewModel(t)).ToList();
                }
            }
            catch (WebException e)
            {
                DependencyManager.Get<NotifierBase>().DisplayToast("Could not connect to server", false);
                Console.WriteLine(e);
            }
            catch (JsonReaderException e)
            {
                DependencyManager.Get<NotifierBase>().DisplayToast("Invalid data received from feed", false);
                Console.WriteLine(e);
            }
            Loaded = true;
        }

        private void ScanForToi()
        {
            if (Loading)
                return;
            Loading = true;

            // for debugging
            _scanner.ScanForToi(new HashSet<string>
            {
                "CC1454015282".TrimStart('0').ToUpper(),
                "FAC4D1038D3D".TrimStart('0').ToUpper(),
                "CBFFB96CA47D".TrimStart('0').ToUpper(),
                "F4B415054205".TrimStart('0').ToUpper()
            });
        }

        public override async void OnViewAppearing()
        {
            base.OnViewAppearing();
            if(_scanner == null)
                _scanner = await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync();
            _scanner.TagsFound += OnTagsFound;
        }

        public override async void OnViewDisappearing()
        {
            base.OnViewDisappearing();
            if (_scanner == null)
                _scanner = await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync();
            _scanner.TagsFound -= OnTagsFound;
        }
    }
}
