using System;
using System.Collections.Generic;
using System.Text;
using TOI_MobileClient.Managers;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public abstract class ViewModelBase : BindableObject
    {
        public ILanguage Language => SettingsManager.Language;
    }
}
