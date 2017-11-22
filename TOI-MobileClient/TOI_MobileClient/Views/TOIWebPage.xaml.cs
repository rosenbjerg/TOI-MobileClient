using TOIClasses;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ToiWebPage : ContentPage
	{
		public ToiWebPage (ToiModel tm)
		{
			InitializeComponent ();
		    BindingContext = new ToiWebViewModel(tm);
		}
	}
}