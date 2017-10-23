using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DepMan;
using TOI_MobileClient.Dependencies;
using Xamarin.Forms;

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
		    DependencyManager.Register<NotifierBase, AndroidNotifier>(new AndroidNotifier());

            global::Xamarin.Forms.Forms.Init (this, bundle);
		    Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.Droid.IconControls.Init(Resource.Id.toolbar, Resource.Id.sliding_tabs);
            LoadApplication (new App ());
		}
    }
}

