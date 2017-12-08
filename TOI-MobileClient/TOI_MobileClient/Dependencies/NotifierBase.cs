using System;
using System.Collections.Generic;
using System.Text;
using Android.App;

namespace TOI_MobileClient.Dependencies
{
    public abstract class NotifierBase
    {
        public abstract void DisplaySnackbar(string text, int dur);
        public abstract void DisplayToast(string text, bool longDuration);
        public abstract void UpdateAppNotification(int bgId, string title, string content, int smallIcon, int largeIcon, bool makeNoice = false);
        public abstract void CancelNotification(int id);
    }
}
