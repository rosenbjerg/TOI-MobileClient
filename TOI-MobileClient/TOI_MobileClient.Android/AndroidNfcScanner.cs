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
            if (intent.Action == NfcAdapter.ActionTagDiscovered)
            {
                var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
                if (tag != null)
                {
                    var rawMessage = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                    if (rawMessage != null)
                    {
                        var message = (NdefMessage)rawMessage[0];

                        var record = message.GetRecords()[0];
                        if (record != null)
                        {
                            return Guid.ParseExact(Encoding.ASCII.GetString(record.GetPayload()), "N");
                        }
                        
                    }
                }
            }
            return Guid.ParseExact("-1".PadLeft(32,'0'), "N");
        }

    }
}