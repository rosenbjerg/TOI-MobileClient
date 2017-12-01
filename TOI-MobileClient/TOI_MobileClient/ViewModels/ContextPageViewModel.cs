using System.Windows.Input;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class ContextPageViewModel : ContextPageViewModelBase
    {
        

        public ContextPageViewModel()
        {
            SaveContext = null;
        }

        public override void SaveContexts()
        {
            SettingsManager.Subscriptions[SettingsManager.Url] = Contexts;
            DependencyManager.Get<NotifierBase>().DisplayToast("Changes have been saved", false);
        }
    }
}