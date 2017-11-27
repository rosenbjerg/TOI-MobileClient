using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
            
            Contexts.ItemSelected += (sender, args) =>
            {
                Contexts.SelectedItem = null;
            };
        }

        protected override void OnDisappearing()
        {
            (BindingContext as ViewModelBase).OnViewDisappearing();
        }

        protected override void OnAppearing()
        {
            (BindingContext as ViewModelBase).OnViewAppearing();
        }

    }
}
