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
        public readonly ToiModel Model;
        public string Title => Model.Title;
        public string ShortDescription => Model.Description;
        public string Image => Model.Image;
        public string Url => Model.Url;

        public ICommand CardTapped { get; }

        public ToiViewModel(ToiModel tf)
        {
            Model = tf;
            CardTapped = new Command(OpenTagCard);
        }

        private void OpenTagCard()
        {
            MainPage.NavigateTo(new ToiWebPage(Model));
        }

        protected bool Equals(ToiViewModel other)
        {
            return Model.Id.Equals(other.Model.Id);
        }

        public override int GetHashCode()
        {
            return Model.Id.GetHashCode();
        }
    }
}