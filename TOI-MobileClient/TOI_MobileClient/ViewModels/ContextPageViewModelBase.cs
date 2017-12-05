using System;
using System.Collections.Generic;
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
    public abstract class ContextPageViewModelBase : PageViewModelBase
    {
        public override string PageTitle => SettingsManager.Language.Contexts;
        
        private bool _loading;
        private List<ContextViewModel> _contexts;
        private IEnumerable<Tuple<string, bool>> _startContexts;
        public bool ContextFetched => _contexts.Count > 0;
        public bool NoContexts => _contexts.Count == 0;
        private bool _allToggled;
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

        private bool _unsavedChanges;
        public bool UnsavedChanges
        {
            get => _unsavedChanges;
            set
            {
                if (_unsavedChanges == value)
                    return;
                _unsavedChanges = value;
                SaveContext = value ? new Command(SaveContexts) : null;
                OnPropertyChanged();

            }
        }
        

        public bool AllToggled
        {
            get => _allToggled;
            set
            {
                _allToggled = value;
                Contexts.ForEach(c => c.Subscribed = _allToggled);
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

        public abstract void SaveContexts();

        public Color SyncColor => Loading ? Styling.DisabledIconColor : Styling.EnabledIconColor;

        protected ContextPageViewModelBase()
        {
            FetchContexts();
            _contexts = new List<ContextViewModel>(); //asyncront kald gør programmet ked af det hvis ikke jeg har en liste at arbejde på
            if (SettingsManager.Subscriptions.TryGetValue(SettingsManager.Url, out var savedContexts))
            {
                _allToggled = savedContexts.All(c => c.Subscribed);
            }
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
                Contexts.ForEach(c => c.Changed += CtxVmOnChanged);
                _startContexts = Contexts.Select(ctx => new Tuple<string, bool>(ctx.Id, ctx.Subscribed)).ToList();
                if (SettingsManager.Subscriptions.TryGetValue(SettingsManager.Url, out var savedContexts))
                {
                    foreach (var sCtxVm in savedContexts)
                    {
                        var ctxVm = Contexts.FirstOrDefault(c => c.Id == sCtxVm.Id);
                        if (ctxVm != null)
                        {
                            ctxVm.Subscribed = sCtxVm.Subscribed;
                        }
                    }
                }
                _startContexts = Contexts.Select(ctx => new Tuple<string, bool>(ctx.Id, ctx.Subscribed)).ToList();
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

        private void CtxVmOnChanged(object sender, EventArgs eventArgs)
        {
            var now = Contexts.Select(ctx => new Tuple<string, bool>(ctx.Id, ctx.Subscribed));
            UnsavedChanges =
                !_startContexts.All(sCtx => now.Any(ctx => ctx.Item1 == sCtx.Item1 && ctx.Item2 == sCtx.Item2));


        }

        public override void OnViewAppearing()
        {
            base.OnViewAppearing();
            if(SettingsManager.Subscriptions.ContainsKey(SettingsManager.Url))
                Contexts = SettingsManager.Subscriptions[SettingsManager.Url];
            
        }
        public override void OnViewDisappearing()
        {
            base.OnViewDisappearing();
            SettingsManager.Subscriptions[SettingsManager.Url] = Contexts;
        }

        private ICommand _saveContext;

        public ICommand SaveContext
        {
            get => _saveContext;
            protected set
            {
                _saveContext = value;
                OnPropertyChanged();
            }
        }
    }
}
