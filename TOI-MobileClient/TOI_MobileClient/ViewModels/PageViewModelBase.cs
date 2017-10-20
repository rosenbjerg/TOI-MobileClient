using System;
using System.Collections.Generic;
using System.Text;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        public abstract string PageTitle
        {
            get;
        }
    }
}
