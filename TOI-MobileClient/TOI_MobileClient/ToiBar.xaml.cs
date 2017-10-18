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
	public partial class ToiBar : ContentView
	{
		public ToiBar ()
		{
			InitializeComponent ();
		}
	}
}