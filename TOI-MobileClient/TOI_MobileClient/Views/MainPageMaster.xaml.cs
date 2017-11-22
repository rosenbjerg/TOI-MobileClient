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
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }
            
            public MainPageMasterViewModel()
            {
                try
                {
                    var lang_2 = SettingsManager.Language;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                var lang = SettingsManager.Language;

                MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem {MenuItemId = 0, Title = lang.ScanForTags, TargetType = typeof(ScanPage)},
                    new MainPageMenuItem {MenuItemId = 1, Title = lang.Settings, TargetType = typeof(SettingsPage)},
                    new MainPageMenuItem {MenuItemId = 2, Title = lang.About}
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}