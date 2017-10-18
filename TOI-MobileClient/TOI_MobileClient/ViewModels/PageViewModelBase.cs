using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    public abstract class PageViewModelBase : BindableObject
    {
        public abstract string PageTitle
        {
            get;
        }
    }
}
