using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosenbjerg.DepMan;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanTestPage : ContentPage
	{
		public ScanTestPage ()
		{
			InitializeComponent ();
		}

	    private async void Button_OnClicked(object sender, EventArgs e)
	    {
	        var scanner = DependencyManager.Get<BleScannerBase>();
	        Console.WriteLine("Scan started");
            var devs = await scanner.ScanDevices();
	        Console.WriteLine("Scan completed");
            var stack = new StackLayout();
            stack.Children.Add(new Label{Text = "Haj med dej"});
            devs.ToList().ForEach(d =>
	        {
	            var but = new Button {Text = d.Address};
	            but.Clicked += (o, args) =>
	            {
	                Console.WriteLine(but.Text + " clicked");
	            };
                stack.Children.Add(but);
	        });
	        Console.WriteLine("Pushing page");
            await Navigation.PushAsync(new ContentPage
	        {
	            Content = stack
	        });
	    }
	}
}