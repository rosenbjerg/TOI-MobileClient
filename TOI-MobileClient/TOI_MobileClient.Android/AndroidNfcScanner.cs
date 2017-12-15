using System;
using System.Collections.Generic;
using System.Linq;
using Android.Nfc;
using Android.App;
using TOI_MobileClient.Dependencies;
using Android.Content;
using System.Text;
using System.Threading.Tasks;
using Android.Telephony;
using Android.Telephony.Gsm;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.Droid
{
    public class AndroidNfcScanner : NfcScannerBase
    {
        public AndroidNfcScanner(NfcAdapter adapter)
        {
            NfcAdapter = adapter;
        }

        public override void HandleNfcIntent(Intent intent)
        {
            if (!SettingsManager.NfcEnabled) return;
            if (intent.Action != NfcAdapter.ActionTagDiscovered || !(intent.GetParcelableExtra(NfcAdapter.ExtraTag) is Tag tag))
                return;

            ResultFound?.Invoke(this, new NfcEventArgs(ByteArrayToString(tag.GetId())));
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public override event EventHandler<NfcEventArgs> ResultFound;

        public new bool IsEnabled => NfcAdapter?.IsEnabled ?? false;
        public NfcAdapter NfcAdapter { get; protected set; }
    }


}