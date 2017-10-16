using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Rosenbjerg.DepMan;
using Android.Nfc;

namespace TOI_MobileClient.Droid
{
    [Activity(Label = "TOI_MobileClient", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private NfcAdapter _nfcAdapter;


        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            DependencyManager.Register<BleScannerBase, AndroidBleScanner>(new AndroidBleScanner());
            global::Xamarin.Forms.Forms.Init(this, bundle);

            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_nfcAdapter == null)
            {
                var alert = new AlertDialog.Builder(this).Create();
                alert.SetMessage("NFC is not supported on this device.");
                alert.SetTitle("NFC Unavailable");
                alert.Show();
            }
            else
            {
                var tagDetected = new  IntentFilter(NfcAdapter.ActionNdefDiscovered);
                var filters = new[] {tagDetected};

                var intent = new Intent(this, this.GetType()).AddFlags(ActivityFlags.SingleTop);

                var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

                _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            _nfcAdapter.DisableForegroundDispatch(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.Action != NfcAdapter.ActionTagDiscovered) return;

            var tag = (Tag) intent.GetParcelableExtra(NfcAdapter.ExtraTag);
            if (tag == null) return;

            var rawMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
            if (rawMessages == null) return;

            var msg = (NdefMessage) rawMessages[0];
            var record = msg.GetRecords()[0];

            if (record?.Tnf == NdefRecord.TnfWellKnown)
            {
                var data = Encoding.UTF8.GetString(record.GetPayload());
                Console.WriteLine("NFC Tag Data");
                Console.WriteLine(data);
            }

        }
    }
}