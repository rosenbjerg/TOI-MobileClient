using System;
using System.Windows.Input;
using TOIClasses;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class ContextViewModel : ViewModelBase
    {
        private readonly ContextModel _model;
        public string Title => _model.Title;
        public string Id => _model.Id;
        public string Description => _model.Description;

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
            _model = cm;
            CardTapped = new Command(ShowDescription);
        }

        private void ShowDescription()
        {
            App.Current.MainPage.DisplayAlert(Title, Description, "OK");
        }
        
        public ICommand CardTapped
        {
            get; private set;
        }

        public event EventHandler Changed;
    }
}
