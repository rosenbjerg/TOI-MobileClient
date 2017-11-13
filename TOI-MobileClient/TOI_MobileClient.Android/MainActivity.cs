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
using TOI_MobileClient.Droid.Services;
using TOI_MobileClient.Localization;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid
{
	[Activity (Label = "TOI_MobileClient", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleInstance)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
	    public static ScannerServiceConnection ServiceConnection;

		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

		    try
		    {
		        base.OnCreate (bundle);
		    }
		    catch (Exception e)
		    {
		        Console.WriteLine(e);
		    }

            DependencyManager.Register<BleScannerBase, AndroidBleScanner>(new AndroidBleScanner());
		    DependencyManager.Register<NotifierBase, AndroidNotifier>(new AndroidNotifier(GetSystemService(Context.NotificationService) as NotificationManager));
		    DependencyManager.Register<ILanguage, EnglishLanguage>(new EnglishLanguage());

            global::Xamarin.Forms.Forms.Init (this, bundle);
		    Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.Droid.IconControls.Init(Resource.Id.toolbar, Resource.Id.sliding_tabs);

		    ServiceConnection = new ScannerServiceConnection();
		    DependencyManager.Register<IScannerServiceProvider, ScannerServiceConnection>(ServiceConnection);

            LoadApplication(new App());
        }

	    protected override void OnDestroy()
	    {
	        base.OnDestroy();
            ServiceConnection.UnbindFromService();
	    }
	}
}

