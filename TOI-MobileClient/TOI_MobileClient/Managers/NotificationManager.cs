using System;
using System.Collections.Generic;
using System.Text;
using Rosenbjerg.DepMan;
using TOI_MobileClient.Dependencies;

namespace TOI_MobileClient.Managers
{
    public class NotificationManager
    {
        public enum NotificationType
        {
            Toast, Snackbar
        }

        private NotifierBase _notifier;

        public NotificationManager()
        {
            _notifier = DependencyManager.Get<NotifierBase>();
        }

        public void Display(string text, NotificationType type, bool longDuration = true)
        {
            switch (type)
            {
                case NotificationType.Toast:
                    _notifier.DisplayToast(text, longDuration);
                    break;
                case NotificationType.Snackbar:
                    _notifier.DisplaySnackbar(text, longDuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
