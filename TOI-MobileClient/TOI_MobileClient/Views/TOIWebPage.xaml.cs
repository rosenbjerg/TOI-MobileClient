using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TOIWebPage : ContentPage
	{
		public TOIWebPage (string url)
		{
			InitializeComponent ();
		    BindingContext = new TOIWebViewModel(url);
		}
	}
}