using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Managers;
using TOI_MobileClient.Models;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SettingsViewModel.UpdateCapabilities();
        }

        private void ListView_OnRefreshing(object sender, EventArgs e)
        {
            SettingsViewModel.UpdateCapabilities();
            ((ListView) sender).IsRefreshing = false;
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.Item == null) return;
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null; // de-select the row
        }
    }

    public class SettingsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RadioTemplate { get; set; }
        public DataTemplate BooleanTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var type = ((SettingViewModel) item).Type;
            switch (type)
            {
                case Setting.SettingType.Boolean:
                    return BooleanTemplate;
                case Setting.SettingType.Radio:
                    return RadioTemplate;
                default:
                    return null;
            }
        }
    }
}