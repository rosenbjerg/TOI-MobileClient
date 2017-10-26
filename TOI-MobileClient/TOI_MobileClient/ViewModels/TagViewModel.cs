using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TOIClasses;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class TagViewModel : ViewModelBase
    {
        private TagInfo _tagInfo;
        public string Title => _tagInfo.Title;
        public string ShortDescription => _tagInfo.Description;
        public string Image => _tagInfo.Image;
        public string Url => _tagInfo.Url;

        public ICommand CardTapped {
            get; private set; 
        }

        public TagViewModel(TagInfo tf)
        {
            _tagInfo = tf;
            CardTapped = new Command(OpenTagWebsite);
        }

        private void OpenTagWebsite()
        {
            Console.WriteLine("Tag card was tapped!");
        }
    }
}
