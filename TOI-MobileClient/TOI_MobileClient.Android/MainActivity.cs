using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Rosenbjerg.DepMan;

namespace TOI_MobileClient.Droid
{
	[Activity (Label = "TOI_MobileClient", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);
		    DependencyManager.Register<BleScannerBase, AndroidBleScanner>(new AndroidBleScanner());
			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new App ());
		}
	}
}

