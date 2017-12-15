using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Telephony.Gsm;
using Android.Views;
using Android.Widget;
using TOI_MobileClient.Dependencies;

namespace TOI_MobileClient.Droid
{
    class AndroidCellularScanner : CellularScannerBase
    {
        private readonly TelephonyManager _telMan;

        public AndroidCellularScanner()
        {
            _telMan = (TelephonyManager)Application.Context.GetSystemService(Context.TelephonyService);

        }

        public override Task ScanAsync()
        {
            if (_telMan.AllCellInfo is List<CellInfo> cellInfos)
            {
                var cigObj = cellInfos.FirstOrDefault(ci => ci is CellInfoGsm);
                if (cigObj == null)
                    return Task.CompletedTask;
                var cig = (CellInfoGsm)cigObj;
                ResultFound?.Invoke(this,
                    new CellularIdFoundEventArgs(cig.CellIdentity.Cid.ToString(),
                        cig.CellIdentity.Lac.ToString()));

                Console.WriteLine($"Cell id found using new API: cid={cig.CellIdentity.Cid} lac={cig.CellIdentity.Lac}");
            }
            else
            {
                // Support for old devices ... 
                if (_telMan.CellLocation is GsmCellLocation gcl)
                {
                    ResultFound?.Invoke(this, new CellularIdFoundEventArgs(gcl.Cid.ToString(), gcl.Lac.ToString()));
                    Console.WriteLine($"Cell id found using old API: cid={gcl.Cid} lac={gcl.Lac}");
                }
            }
            return Task.CompletedTask;
        }

        public override event EventHandler<CellularIdFoundEventArgs> ResultFound;
    }
}