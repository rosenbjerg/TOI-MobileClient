﻿using System;
using System.Collections.Generic;
using System.Text;
using Android.App;

namespace TOI_MobileClient.Dependencies
{
    public abstract class NotifierBase
    {
        public abstract void DisplaySnackbar(string text, bool longDuration);
        public abstract void DisplayToast(string text, bool longDuration);
        public abstract void DisplayStatusNotification();
    }
}
