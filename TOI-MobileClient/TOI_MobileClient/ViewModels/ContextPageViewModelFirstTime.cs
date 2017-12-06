using System.Collections.Generic;
using System.Linq;
using DepMan;
using TOIClasses;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.ViewModels
{
    public class ContextPageViewModelFirstTime : ContextPageViewModelBase
    {
        public ContextPageViewModelFirstTime()
        {

        }

        public override async void SaveContexts()
        {
            App.Current.MainPage = new MainPage();
            App.Navigation = App.Current.MainPage.Navigation;
            SettingsManager.Subscriptions[SettingsManager.Url] = Contexts;
            var ids = Contexts.Where(c => c.Subscribed).Select(c => c.Id).ToList();
            var dict = new Dictionary<string, string>
            {
                {"contexts", string.Join(",", ids)}
            };
            var res = await DependencyManager.Get<RestClient>().GetMany<ToiModel>(SettingsManager.Url + "/tois", dict);
            var tags = res.SelectMany(r => r.Tags);
            SettingsManager.ToiFilter = tags.ToHashSet();

        }
    }
}