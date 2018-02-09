using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TOI_MobileClient.Managers;
using TOI_MobileClient.Views;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class FeedSelectionViewModel : PageViewModelBase
    {
        public ObservableCollection<FeedServerViewModel> FeedServers { get; } =
            new ObservableCollection<FeedServerViewModel>();

        public FeedSelectionViewModel()
        {
            // TODO: Fetch from FeedRepo
            FeedServers.Add(new FeedServerViewModel
            {
                Name = "Jespers Server",
                Description = "En fin test server til at lave TOIs og kontekster på. Denne server har en meget lang description, som fylder en masse, for at vi kan teste lange descriptions. Fedt man, spa!",
                BaseUrl = "http://ssh.windelborg.info:7474"
            });
        }

        public override string PageTitle => SettingsManager.Language.SelectFeedServer;
    }

    public class FeedServerViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BaseUrl { get; set; }

        public ICommand Tapped { get; }
        public ICommand Info { get; }

        public FeedServerViewModel()
        {
            Tapped = new Command(RedirectContext);
            Info = new Command(ShowDescription);
        }

        private async void RedirectContext()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(
                new ContextPage(Name, BaseUrl));
        }

        private void ShowDescription()
        {
            App.Current.MainPage.DisplayAlert(Name, Description, "OK");
        }
    }
}