using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TOIClasses;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class ToiWebViewModel : ViewModelBase
    {
        private WebViewSource _sows;

        public WebViewSource BrowserSource
        {
            get { return _sows; }
            private set
            {
                if (_sows != null && value == _sows)
                    return;
                _sows = value;
                OnPropertyChanged();
            }
        }

        public ToiWebViewModel(ToiModel toi)
        {
            switch (toi.InformationType)
            {
                case ToiInformationType.Image:
                    BrowserSource = new HtmlWebViewSource
                    {
                        Html = HtmlViewBuilder.BuildImageView(toi.Url)
                    };
                    break;
                case ToiInformationType.Audio:
                    BrowserSource = new HtmlWebViewSource
                    {
                        Html = HtmlViewBuilder.BuildAudioView(toi.Url)
                    };
                    break;
                case ToiInformationType.Video:
                    BrowserSource = new HtmlWebViewSource
                    {
                        Html = HtmlViewBuilder.BuildVideoView(toi.Url)
                    };
                    break;
                case ToiInformationType.Text:
                    BrowserSource = new HtmlWebViewSource
                    {
                        Html = HtmlViewBuilder.BuildTextView(toi.Url)
                    };
                    break;
                case ToiInformationType.Website:
                default:
                    BrowserSource = new UrlWebViewSource
                    {
                        Url = toi.Url
                    };
                    break;
            }
        }
    }
}
