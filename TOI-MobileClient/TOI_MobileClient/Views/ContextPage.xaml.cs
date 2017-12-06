using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContextPage : ContentPage
    {
        public string Name { get; }
        public string BaseUrl { get; }

        public ContextPage(string name, string url)
        {
            Name = name;
            BaseUrl = url;
            BindingContext = new ContextPageViewModel(Name, BaseUrl);
            InitializeComponent();

            ContextList.ItemSelected += (sender, args) =>
            {
                ContextList.SelectedItem = null;
            };
        }

        protected override void OnDisappearing()
        {
            ((PageViewModelBase) BindingContext).OnViewDisappearing();
        }

        protected override void OnAppearing()
        {
            ((PageViewModelBase) BindingContext).OnViewAppearing();
        }


    }
}
