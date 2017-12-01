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
        public ContextPage()
        {
            InitializeComponent();
            BindingContext = new ContextPageViewModel();
            Contexts.ItemSelected += (sender, args) =>
            {
                Contexts.SelectedItem = null;
            };
        }
        public ContextPage(ContextPageViewModelBase cp)
        {
            
            InitializeComponent();
            BindingContext = cp;
            Contexts.ItemSelected += (sender, args) =>
            {
                Contexts.SelectedItem = null;
            };
        }

        protected override void OnDisappearing()
        {
            (BindingContext as PageViewModelBase).OnViewDisappearing();
        }

        protected override void OnAppearing()
        {
            (BindingContext as PageViewModelBase).OnViewAppearing();
        }

    }
}
