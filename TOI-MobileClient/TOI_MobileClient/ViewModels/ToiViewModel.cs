using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TOIClasses;
using TOI_MobileClient.Views;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class ToiViewModel : ViewModelBase
    {
        private readonly ToiModel _info;
        public string Title => _info.Title;
        public string ShortDescription => _info.Description;
        public string Image => _info.Image;
        public string Url => _info.Url;

        public ICommand CardTapped {
            get; private set; 
        }

        public ToiViewModel(ToiModel tf)
        {
            _info = tf;
            CardTapped = new Command(OpenTagCard);
        }

        private void OpenTagCard()
        {
            MainPage.NavigateTo(new ToiWebPage(_info));
        }
    }
}
