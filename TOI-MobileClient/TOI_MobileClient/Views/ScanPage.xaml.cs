using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            NearbyTags.ItemSelected += (sender, args) =>
            {
                NearbyTags.SelectedItem = null;
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