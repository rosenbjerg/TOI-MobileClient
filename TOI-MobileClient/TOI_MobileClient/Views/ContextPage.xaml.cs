using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContextPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ContextPage()
        {
            InitializeComponent();

        }

    }
}
