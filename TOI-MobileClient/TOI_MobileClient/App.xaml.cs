using System.Collections.Generic;
using System.Linq;
using Rosenbjerg.DepMan;

namespace TOI_MobileClient
{
	public partial class App : Xamarin.Forms.Application
	{
		public App ()
		{
			InitializeComponent();
		    var nav = new MainPage();
            MainPage = nav;
		    nav.PushAsync(new ScanTestPage());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
