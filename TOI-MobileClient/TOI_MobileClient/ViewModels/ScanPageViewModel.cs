using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public ObservableCollection<ToiViewModel> ToiCollection { get; set; }

        public bool FoundTags => ToiCollection.Count > 0;
        public bool NoTags => ToiCollection.Count == 0;
        public Color SyncColor => Loading ? Styling.DisabledIconColor : Styling.EnabledIconColor;

        public ViewStates SyncBtnVisibility => SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never
            ? ViewStates.Gone
            : ViewStates.Visible;

        public ViewStates ClearBtnVisibility => SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never
            ? ViewStates.Visible
            : ViewStates.Gone;

        public bool PullToRefresh =>
            SettingsManager.ScanFrequencyValue == SettingsManager.Language.Never;

        private IBackgroundScanner _scanner;

        public ScanPageViewModel()
        {
            SyncCommand = new Command(Sync);
            ClearCommand = new Command(() =>
            {
                TagCache.Clear();
                ToiCache.Clear();
                ToiCollection.Clear();
                OnPropertyChanged(nameof(FoundTags));
                OnPropertyChanged(nameof(NoTags));
            });
            ToiCollection = new ObservableCollection<ToiViewModel>();
        }

        private HashSet<string> TagCache { get; } = new HashSet<string>();
        private HashSet<ToiModel> ToiCache { get; } = new HashSet<ToiModel>();

        private async void TagFound(object sender, TagFoundEventArgs args)
        {
            if (TagCache.Contains(args.Tag)) return;

            var rc = DependencyManager.Get<RestClient>();

            try
            {
                var tois = await rc.PostMany<ToiModel>(
                    SettingsManager.Url + (args.Gps ? "/toi/fromgps" : "/toi/fromtags"), new List<string> {args.Tag});

                tois?.ForEach(t =>
                {
                    ToiCache.Add(t);
                    var vm = new ToiViewModel(t);
                    if (ToiCollection.All(v => v.Model.Id != vm.Model.Id))
                    {
                        ToiCollection.Add(vm);
                    }
                    TagCache.Add(args.Tag);

                    OnPropertyChanged(nameof(FoundTags));
                    OnPropertyChanged(nameof(NoTags));
                });
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
            catch (HttpRequestException e)
            {
                DependencyManager.Get<NotifierBase>().DisplayToast("Server is offline!", false);
                Console.WriteLine(e);
            }
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