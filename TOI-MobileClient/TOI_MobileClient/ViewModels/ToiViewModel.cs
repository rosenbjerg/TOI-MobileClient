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
        private readonly ToiModel _model;
        public string Title => _model.Title;
        public string ShortDescription => _model.Description;
        public string Image => _model.Image;
        public string Url => _model.Url;

        public ICommand CardTapped {
            get; private set; 
        }

        public ToiViewModel(ToiModel tf)
        {
            _model = tf;
            CardTapped = new Command(OpenTagCard);
        }

        private void OpenTagCard()
        {
            MainPage.NavigateTo(new ToiWebPage(_model));
        }
    }
}
