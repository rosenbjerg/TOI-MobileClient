using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    class TOIWebView : WebView
    {
        public static readonly BindableProperty GoBackCommandProperty = BindableProperty.Create(nameof(GoBackCommand), typeof(ICommand), typeof(TOIWebView));
        public ICommand GoBackCommand
        {
            get => (ICommand)GetValue(GoBackCommandProperty);
            set => SetValue(GoBackCommandProperty, value);
        }

        public static readonly BindableProperty GoForwardCommandProperty = BindableProperty.Create(nameof(GoForwardCommand), typeof(ICommand), typeof(TOIWebView));
        public ICommand GoForwardCommand
        {
            get => (ICommand)GetValue(GoForwardCommandProperty);
            set => SetValue(GoForwardCommandProperty, value);
        }

        public static readonly BindableProperty BackColorProperty = BindableProperty.Create(nameof(BackColor), typeof(Color), typeof(TOIWebView), Styling.DisabledIconColor);
        public Color BackColor
        {
            get => (Color)GetValue(BackColorProperty);
            set => SetValue(BackColorProperty, value);
        }
        public static readonly BindableProperty ForwardColorProperty = BindableProperty.Create(nameof(ForwardColor), typeof(Color), typeof(TOIWebView), Styling.DisabledIconColor);
        public Color ForwardColor
        {
            get => (Color)GetValue(ForwardColorProperty);
            set => SetValue(ForwardColorProperty, value);
        }

        public TOIWebView()
        {
            GoBackCommand = new Command(() =>
            {
                GoBack();
                updateButtonColors();
            });
            GoForwardCommand = new Command(() =>
            {
                GoForward();
                updateButtonColors();
            });

            //Subscribe a listener to the "Navigated" event, it updates the colors of the navigation buttons after a short delay
            Navigated += async delegate
            {
                await Task.Delay(100);
                updateButtonColors();
            };
        }
        

        private void updateButtonColors()
        {
            BackColor = CanGoBack ? Styling.EnabledIconColor : Styling.DisabledIconColor;
            ForwardColor = CanGoForward ? Styling.EnabledIconColor : Styling.DisabledIconColor;
        }
    }
}
