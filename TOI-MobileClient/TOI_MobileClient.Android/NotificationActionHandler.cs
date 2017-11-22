using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TOI_MobileClient.Droid.Services;

namespace TOI_MobileClient.Droid
{
    class NotificationActionHandler : BroadcastReceiver
    {
        public NotificationActionHandler(MainActivity activity)
        {
            var iFilter = new IntentFilter(NotificationActionHandler.KILL_APP_AND_SERVICE);
            activity.RegisterReceiver(this, iFilter);
        }
        
        public const string KILL_APP_AND_SERVICE = "android.toi.CLOSE_APP";

        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine("Received a broadcast: " + intent.Action);
            switch (intent.Action)
            {
                case KILL_APP_AND_SERVICE:
                    var act = (context as Activity);
                    Console.WriteLine("context as Activity: " + act);
                    var b = context.StopService(new Intent(context, typeof(ToiScannerService)));
                    act.FinishActivity(0);
                    break;
            }
        }
    }
}