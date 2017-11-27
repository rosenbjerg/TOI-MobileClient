using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Input;
using Android.Opengl;
using Android.Views;
using DepMan;
using Newtonsoft.Json;
using TOIClasses;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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


        public ObservableCollection<ToiViewModel> ToiCollection { get; set; }


        public bool FoundTags => ToiCollection.Count > 0;

        public bool NoTags => ToiCollection.Count == 0;

        public Color SyncColor => Loading ? Styling.DisabledIconColor : Styling.EnabledIconColor;

        public ViewStates Visibility => SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never ? ViewStates.Gone : ViewStates.Visible;

        public bool PullToRefresh =>
            SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never;

        private IBackgroundScanner _scanner;

        public ScanPageViewModel()
        {
            SyncCommand = new Command(ScanForToi);
            ToiCollection = new ObservableCollection<ToiViewModel>();
        }

        private HashSet<string> TagCache { get; set; } = new HashSet<string>();
        private HashSet<ToiModel> ToiCache { get; set; } = new HashSet<ToiModel>();

        private async void TagFound(object sender, TagFoundEventArgs args)
        {
            Loading = false;
            if (TagCache.Contains(args.Tag)) return;

            var rc = DependencyManager.Get<RestClient>();

            try
            {
                var tois = await rc.GetMany<ToiModel>(SettingsManager.Url + "/toi/fromtags",
                    new List<string> {args.Tag});
                tois?.ForEach(t =>
                {
                    ToiCache.Add(t);
                    var vm = new ToiViewModel(t);
                    if (ToiCollection.All(v => v.Model.Id != vm.Model.Id))
                    {
                        ToiCollection.Insert(0, vm);
                    }
                    TagCache.Add(args.Tag);
                });

                OnPropertyChanged(null);
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
            OnPropertyChanged(null);

            if (_scanner == null)
                _scanner = await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync();

            _scanner.TagFound += TagFound;
        }
        
        public override async void OnViewDisappearing()
        {
            if (_scanner == null)
                _scanner = await DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync();

            _scanner.TagFound -= TagFound;
        }
    }
}