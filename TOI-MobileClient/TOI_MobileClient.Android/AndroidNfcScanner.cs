using System;
using Android.Nfc;
using Android.App;
using TOI_MobileClient.Dependencies;
using Android.Content;
using System.Text;

namespace TOI_MobileClient.Droid
{
    public class AndroidNfcScanner : NfcScannerBase
    {
        private NfcAdapter _nfcAdapter;
        
        public AndroidNfcScanner()
        {
            _nfcAdapter = NfcAdapter.GetDefaultAdapter(Application.Context);
        }

        public override Guid ScanNfc(Intent intent)
        {
            if (intent.Action != NfcAdapter.ActionTagDiscovered) return Guid.ParseExact("".PadLeft(32, '0'), "N");
            if (!(intent.GetParcelableExtra(NfcAdapter.ExtraTag) is Tag tag))
                return Guid.ParseExact("".PadLeft(32, '0'), "N");

            var result = Guid.ParseExact(ByteArrayToString(tag.GetId()).PadLeft(32, '0'), "N");
            return Guid.ParseExact(ByteArrayToString(tag.GetId()).PadLeft(32, '0'), "N");
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}