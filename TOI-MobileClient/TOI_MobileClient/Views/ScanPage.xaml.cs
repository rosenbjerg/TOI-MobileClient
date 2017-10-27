using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //Remove the ugly orange border around the selected Card
            NearbyTags.ItemSelected += (sender, args) =>
            {
                NearbyTags.SelectedItem = null;
            };
        }
    }
}