using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.Managers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeedSelectionPage : ContentPage
	{
		public FeedSelectionPage ()
		{
			InitializeComponent ();

		    FeedList.ItemSelected += (sender, args) =>
		    {
		        FeedList.SelectedItem = null;
		    };
        }
	}
}