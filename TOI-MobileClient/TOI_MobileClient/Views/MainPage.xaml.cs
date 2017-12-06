using System;
using System.Collections.Generic;
using FormsPlugin.Iconize;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage
    {
        public static Action<Page> NavigateTo;

        public MainPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            // Caching and loading of front page
            var firstPage = new IconNavigationPage(new ScanPage());
            _loadedPages.Add(typeof(ScanPage), firstPage);
            Detail = firstPage;

            if (firstPage.CurrentPage.BindingContext is PageViewModelBase vm)
            {
                vm.OnViewAppearing();
            }

            NavigateTo = delegate(Page page)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (Detail is IconNavigationPage iPage && iPage.CurrentPage.BindingContext is PageViewModelBase pvm)
                    {
                        pvm.OnViewDisappearing();
                    }

                    (Detail as IconNavigationPage)?.PushAsync(page);

                    if (page.BindingContext is PageViewModelBase pageViewModel)
                    {
                        pageViewModel.OnViewAppearing();
                    }
                    //Detail = new IconNavigationPage(page);
                });
            };
        }

        // Page cache
        private readonly Dictionary<Type, IconNavigationPage> _loadedPages = new Dictionary<Type, IconNavigationPage>();

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MainPageMenuItem) e.SelectedItem;
            if (item == null)
                return;
            if (!_loadedPages.TryGetValue(item.TargetType, out var page))
            {
                var innerPage = (Page) Activator.CreateInstance(item.TargetType);
                innerPage.Title = item.Title;
                page = new IconNavigationPage(innerPage);
                _loadedPages.Add(item.TargetType, page);
            }

            Detail = page;

            if (page.CurrentPage.BindingContext is PageViewModelBase vm)
            {
                vm.OnViewAppearing();
            }

            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}