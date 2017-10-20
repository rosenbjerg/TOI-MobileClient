using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using TOI_MobileClient.Dependencies;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace TOI_MobileClient.Droid
{
    class AndroidNotifier : NotifierBase
    {
        public override void DisplaySnackbar(string text, bool longDur = true)
        {
            var activity = (Activity)Forms.Context;
            var view = activity.FindViewById(Android.Resource.Id.Content);

            Device.BeginInvokeOnMainThread(() =>
            {
                Snackbar.Make(view, text, longDur ? 10000 : 3000).Show();
            });
        }

        public override void DisplayToast(string text, bool longDur = true)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Forms.Context, text, longDur ? ToastLength.Long : ToastLength.Short).Show();
            });
            
        }
    }
}