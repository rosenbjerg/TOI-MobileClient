using System;
using System.Windows.Input;
using TOIClasses;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class ContextViewModel : ViewModelBase
    {
        public readonly ContextModel Model;
        public string Title => Model.Title;
        public string Id => Model.Id;
        public string Description => Model.Description;

        private bool _subscribed;

        public bool Subscribed
        {
            get => _subscribed;
            set
            {
                if (_subscribed == value)
                    return;
                _subscribed = value;
                OnPropertyChanged();
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        public ContextViewModel(ContextModel cm)
        {
            Model = cm;
            CardTapped = new Command(ShowDescription);
        }

        private void ShowDescription()
        {
            App.Current.MainPage.DisplayAlert(Title, Description, "OK");
        }

        public ICommand CardTapped { get; }

        public event EventHandler Changed;
    }
}