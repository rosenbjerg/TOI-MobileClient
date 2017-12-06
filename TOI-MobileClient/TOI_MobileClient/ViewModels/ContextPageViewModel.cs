using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Views;
using DepMan;
using Newtonsoft.Json;
using TOIClasses;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TOI_MobileClient.ViewModels
{
    public class ContextPageViewModel : PageViewModelBase
    {
        public override string PageTitle => SettingsManager.Language.Contexts;

        public ObservableCollection<ContextViewModel> Contexts { get; set; } =
            new ObservableCollection<ContextViewModel>();

        public bool ContextFetched => Contexts.Count > 0;
        public bool NoContexts => Contexts.Count == 0;
        private bool _allToggled;

        public bool AllToggled
        {
            get => _allToggled;
            set
            {
                _allToggled = value;
                Contexts.ForEach(c => c.Subscribed = _allToggled);
                OnPropertyChanged();
            }
        }

        public string Name { get; }
        public string BaseUrl { get; }
        public ICommand SaveContext { get; }

        public ContextPageViewModel(string name, string baseUrl)
        {

            Name = name;
            BaseUrl = baseUrl;

            FetchContexts();

            SaveContext = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();

                var ctxs = Contexts.Where(c => c.Subscribed).Select(c => c.Id).ToList();

                if (ctxs.Count == 0)
                {
                    SubscriptionManager.Instance.RemoveServer(BaseUrl);
                }
                else if (SubscriptionManager.Instance.SubscribedServers.TryGetValue(BaseUrl, out var ss))
                {
                    ss.Contexts = ctxs;
                }
                else
                {
                    SubscriptionManager.Instance.AddServer(Name, BaseUrl, ctxs);
                }
                DependencyManager.Get<NotifierBase>().DisplayToast(SettingsManager.Language.ChangesSaved, false);
            });
        }

        private async void FetchContexts()
        {
            var contexts = await ToiHttpClient.Instance.GetMany<ContextModel>(BaseUrl + "/contexts");
            Contexts = new ObservableCollection<ContextViewModel>(contexts.Select(c => new ContextViewModel(c)
            {
                Subscribed = SubscriptionManager.Instance.SubscribedServers.TryGetValue(BaseUrl, out var ss) &&
                             ss.Contexts.Contains(c.Id)
            }));
            OnPropertyChanged(nameof(Contexts));
            if (Contexts.All(c => c.Subscribed))
            {
                AllToggled = true;
            }
        }
    }
}