using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DepMan;
using TOIClasses;
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

        public override async void SaveContexts()
        {
            SettingsManager.Subscriptions[SettingsManager.Url] = Contexts;
            var ids = Contexts.Where(c => c.Subscribed).Select(c => c.Id).ToList();
            var dict = new Dictionary<string, string>
            {
                {"contexts", string.Join(",", ids)}
            };
            var res = await DependencyManager.Get<RestClient>().GetMany<ToiModel>(SettingsManager.Url + "/tois", dict);
            var tags = res.SelectMany(r => r.Tags);
            SettingsManager.ToiFilter = tags.ToHashSet();
            DependencyManager.Get<NotifierBase>().DisplayToast("Changes have been saved", false);
        }
    }
}