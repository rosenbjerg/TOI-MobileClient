using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Droid.Services;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid
{
	[Activity (Label = "Things of Interest", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
	    public static ScannerServiceConnection ServiceConnection;
	    public NotificationActionHandler NotificationActionHandler { get; private set; }
	    private AndroidNfcScanner _nfcScanner;

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
		    DependencyManager.Register<NotifierBase, AndroidNotifier>(new AndroidNotifier(GetSystemService(NotificationService) as NotificationManager));
		    DependencyManager.Register<GpsLocatorBase, AndroidGpsScanner>(new AndroidGpsScanner());
		    DependencyManager.Register<WiFiScannerBase, AndroidWifiScanner>(new AndroidWifiScanner());
            _nfcScanner = new AndroidNfcScanner(NfcAdapter.GetDefaultAdapter(this));
		    DependencyManager.Register<NfcScannerBase, AndroidNfcScanner>(_nfcScanner);

            Forms.Init (this, bundle);
		    Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.Droid.IconControls.Init(Resource.Id.toolbar, Resource.Id.sliding_tabs);
            
		    ServiceConnection = new ScannerServiceConnection();
		    DependencyManager.Register<IScannerServiceProvider, ScannerServiceConnection>(ServiceConnection);

		    NotificationActionHandler = new NotificationActionHandler(this);

		    LoadApplication(new App());

        }

        protected override void OnResume()
	    {
            base.OnResume();

            var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
	        var filters = new[] { tagDetected };

	        var intent = new Intent(this, this.GetType()).AddFlags(ActivityFlags.SingleTop);

	        var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);
	        _nfcScanner.NfcAdapter?.EnableForegroundDispatch(this, pendingIntent, filters, null);
	    }

        protected override void OnNewIntent(Intent intent)
	    {
	        DependencyManager.Get<NfcScannerBase>().HandleNfcIntent(intent);
	    }

	    protected override void OnDestroy()
	    {
	        base.OnDestroy();
            DependencyManager.Get<NotifierBase>().CancelNotification(DependencyManager.Get<IScannerServiceProvider>().GetServiceAsync().Id);
            ServiceConnection.UnbindFromService();
	    }
	}
}

