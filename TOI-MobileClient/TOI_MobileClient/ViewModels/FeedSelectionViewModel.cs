using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TOI_MobileClient.Managers;
using TOI_MobileClient.Views;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    class FeedSelectionViewModel
    {
        public ICommand RedirectToContext { get; }

        public FeedSelectionViewModel()
        {
            RedirectToContext = new Command(RedirectContext);
        }

        private void RedirectContext()
        {
            if (!SettingsManager.Subscriptions.ContainsKey(SettingsManager.Url) ||
                SettingsManager.Subscriptions[SettingsManager.Url].Count == 0)
            {
                Application.Current.MainPage = new ContextPage(new ContextPageViewModelFirstTime());
            }
        }
    }
}
