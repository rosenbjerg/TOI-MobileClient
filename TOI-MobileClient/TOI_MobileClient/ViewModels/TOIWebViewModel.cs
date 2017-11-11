using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class TOIWebViewModel : ViewModelBase
    {
        private UrlWebViewSource _sows;

        public UrlWebViewSource BrowserSource
        {
            get { return _sows; }
            private set
            {
                if (_sows != null && value.Url == _sows.Url)
                    return;
                _sows = value;
                OnPropertyChanged();
            }
        }

        public TOIWebViewModel(string url)
        {
            BrowserSource = new UrlWebViewSource {Url = url};
        }
    }
}
