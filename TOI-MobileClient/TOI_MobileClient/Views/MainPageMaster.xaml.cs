using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TOI_MobileClient.Managers;
using TOI_MobileClient.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster
    {
        public ListView ListView;
        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        public class MainPageMasterViewModel : BindableObject
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                var lang = SettingsManager.Language;

                MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem {MenuItemId = 0, Title = lang.ScanForTags, TargetType = typeof(ScanPage)},
                    new MainPageMenuItem {MenuItemId = 1, Title = lang.Feeds, TargetType = typeof(FeedSelectionPage)},
                    new MainPageMenuItem {MenuItemId = 2, Title = lang.Settings, TargetType = typeof(SettingsPage)},
                });
            }

        }
    }
}