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
        
        public AndroidNfcScanner()
        {
           
        }


        public override void HandleNfcIntent(Intent intent)
        {
            if (intent.Action != NfcAdapter.ActionTagDiscovered || !(intent.GetParcelableExtra(NfcAdapter.ExtraTag) is Tag tag))
                return;

            NfcTagFound?.Invoke(this, new NfcEventArgs(ByteArrayToString(tag.GetId())));
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