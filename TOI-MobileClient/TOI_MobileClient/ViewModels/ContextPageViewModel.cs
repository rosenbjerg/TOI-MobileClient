using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using DepMan;
using Newtonsoft.Json;
using TOIClasses;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    class ContextPageViewModel : PageViewModelBase
    {
        public override string PageTitle => SettingsManager.Language.Contexts;

        public ICommand SyncCommand { get; }

        private bool _loading;
        private List<ContextViewModel> _contexts;
        public List<ContextViewModel> SelectedModels { get; set; }
        public bool ContextFetched => _contexts.Count > 0;
        public bool NoContexts => _contexts.Count == 0;
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

        public List<ContextViewModel> Contexts
        {
            get => _contexts;
            set
            {
                if (value == _contexts)
                    return;
                _contexts = value;
                OnPropertyChanged(nameof(ContextFetched));
                OnPropertyChanged(nameof(NoContexts));
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
        public Color SyncColor => Loading ? Styling.DisabledIconColor : Styling.EnabledIconColor;

        public ContextPageViewModel()
        {
            SyncCommand = new Command(FetchContexts);
            _contexts = new List<ContextViewModel>();

        }

        private async void FetchContexts()
        {
            if (Loading)
                return;
            Loading = true;
            try
            {
                var cm = (await DependencyManager.Get<RestClient>()
                    .GetMany<ContextModel>(SettingsManager.Url + "/contexts")).ToList();
                Contexts = cm.Select(t => new ContextViewModel(t)).ToList();
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
        public override void OnViewAppearing()
        {
            base.OnViewDisappearing();
            if(SettingsManager.Subscriptions.ContainsKey(SettingsManager.Url))
                Contexts = SettingsManager.Subscriptions[SettingsManager.Url];       
        }
        public override void OnViewDisappearing()
        {
            base.OnViewDisappearing();
            SettingsManager.Subscriptions[SettingsManager.Url] = Contexts.Where(t => t.Subscribed).ToList();
        }
    }
}
