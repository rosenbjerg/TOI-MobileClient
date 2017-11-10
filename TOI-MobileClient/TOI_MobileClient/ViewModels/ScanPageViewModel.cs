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

        public Color SyncColor => Loading ? Styling.DisabledIconColor : Styling.EnabledIconColor;

        private IBackgroundScanner _scanner;

        public ScanPageViewModel()
        {
            SyncCommand = new Command(ScanForToi);
            NearbyTags = new List<TagViewModel>();
        }
        

        private async void OnTagsFound(object sender, TagsFoundsEventArgs tagsFoundsEventArgs)
        {
            tagsFoundsEventArgs.Handled = true;
            var rc = DependencyManager.Get<RestClient>();
            try
            {
                var tvms = await rc.GetMany<TagInfo>(SettingsManager.Url, tagsFoundsEventArgs.Tags);
                if (tvms == null)
                {
                    NearbyTags = new List<TagViewModel>();
                    DependencyManager.Get<NotificationManager>().Display("Could not connect to server",
                        NotificationManager.NotificationType.Toast);
                }
                else
                    NearbyTags = tvms.Select(t => new TagViewModel(t)).ToList();
            }
            catch (WebException e)
            {
                DependencyManager.Get<NotificationManager>().Display("Could not connect to server",
                    NotificationManager.NotificationType.Toast);
                Console.WriteLine(e);
            }
            catch (JsonReaderException e)
            {
                DependencyManager.Get<NotificationManager>().Display("Invalid data received from feed",
                    NotificationManager.NotificationType.Toast);
                Console.WriteLine(e);
            }

            Loaded = true;
        }

        private void ScanForToi()
        {
            if (Loading)
                return;
            Loading = true;
            _scanner.ScanForToi(new HashSet<Guid>
            {
                Guid.ParseExact("cc1454015282".PadLeft(32, '0'), "N"),
                Guid.ParseExact("FAC4D1038D3D".PadLeft(32, '0'), "N"),
                Guid.ParseExact("CBFFB96CA47D".PadLeft(32, '0'), "N"),
                Guid.ParseExact("F4B415054205".PadLeft(32, '0'), "N")
            });
        }

        public override async void OnViewAppearing()
        {
            base.OnViewAppearing();
            if(_scanner == null)
                _scanner = await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync();
            _scanner.TagsFound += OnTagsFound;
        }

        public override void OnViewDisappearing()
        {
            base.OnViewDisappearing();
            _scanner.TagsFound -= OnTagsFound;
        }
    }
}
