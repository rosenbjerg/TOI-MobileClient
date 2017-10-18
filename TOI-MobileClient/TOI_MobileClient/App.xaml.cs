using System.Collections.Generic;
using System.Linq;
using Rosenbjerg.DepMan;
using Xamarin.Forms;

namespace TOI_MobileClient
{
	public partial class App : Xamarin.Forms.Application
	{
		public App ()
		{
			InitializeComponent();
            MainPage = new MainPage();
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
