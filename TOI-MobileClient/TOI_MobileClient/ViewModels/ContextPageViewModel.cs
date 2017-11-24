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

        private List<ContextModel> _contexts;
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

        }

        private async void FetchContexts()
        {
            try
            {
                _contexts = (await DependencyManager.Get<RestClient>()
                    .GetMany<ContextModel>(SettingsManager.Url + "/contexts")).ToList();
                
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
    }
}
