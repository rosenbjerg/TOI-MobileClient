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
			MainPage = new MainPage();
		}

	    public async void test()
	    {
	        var tags = new List<string> {"jonas"};
	        var scanner = DependencyManager.Get<BleScannerBase>();
	        var devs = await scanner.ScanDevices();
	        devs.Where(d => tags.Contains(d.Address));

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
