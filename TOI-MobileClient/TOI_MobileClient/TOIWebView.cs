using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    class TOIWebView : WebView
    {
        public static readonly BindableProperty GoBackCommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TOIWebView), null);

        public ICommand GoBackCommand
        {
            get => (ICommand)GetValue(GoBackCommandProperty);
            set => SetValue(GoBackCommandProperty, value);
        }

        public static readonly BindableProperty GoForwardCommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TOIWebView), null);

        public ICommand GoForwardCommand
        {
            get => (ICommand)GetValue(GoForwardCommandProperty);
            set => SetValue(GoForwardCommandProperty, value);
        }

        public TOIWebView()
        {
            GoBackCommand = new Command(GoBack);
            GoForwardCommand = new Command(GoForward);
        }
    }
}
