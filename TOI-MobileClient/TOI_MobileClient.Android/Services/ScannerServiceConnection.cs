using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using DepMan;
using TOI_MobileClient.Dependencies;
using Xamarin.Forms;

namespace TOI_MobileClient.Droid.Services
{
    public class ScannerServiceConnection : Java.Lang.Object, IServiceConnection, IScannerServiceProvider
    {
        private TaskCompletionSource<ToiScannerService> _tsc;

        public ScannerServiceBinder Binder { get; private set; }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            if (!(service is ScannerServiceBinder scannerServiceBinder)) return;
            Binder = scannerServiceBinder;
            var s = Binder.GetService();
            _tsc.SetResult(s);
            s.StartBackgroundScanning();
        }

        public async Task<IBackgroundScanner> GetServiceAsync()
        {
            if (Binder != null)
                return Binder.GetService();
            _tsc = new TaskCompletionSource<ToiScannerService>();
            var intent = CreateServiceIntent();
            Android.App.Application.Context.ApplicationContext.BindService(intent, MainActivity.ServiceConnection,
                Bind.AboveClient);

            return await _tsc.Task;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            Binder = null;
        }

        public Intent CreateServiceIntent()
        {
            var serviceIntent =
                new Intent(Android.App.Application.Context.ApplicationContext, typeof(ToiScannerService));

            Android.App.Application.Context.ApplicationContext.StartService(serviceIntent);

            return serviceIntent;
        }

        //private bool IsServiceRunning()
        //{
        //    var manager = (ActivityManager)Forms.Context.ApplicationContext.GetSystemService(Context.ActivityService);
        //    var runningServices = manager.GetRunningServices(int.MaxValue);

        //    string name = nameof(ToiScannerService);
        //    return runningServices.Any(service => service.Service.ClassName.Contains(name));
        //}

        public void UnbindFromService()
        {
            if (Binder != null)
            {
                Android.App.Application.Context.ApplicationContext.UnbindService(this);
            }
        }
    }
}