using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TOI_MobileClient.Dependencies;

namespace TOI_MobileClient.Droid
{
    class AndroidCellularScanner : IHardware, IScanner<CellularIdFoundEventArgs>
    {
        public bool IsEnabled { get; } = true;
        public Task ScanAsync()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<CellularIdFoundEventArgs> ResultFound;
    }
}