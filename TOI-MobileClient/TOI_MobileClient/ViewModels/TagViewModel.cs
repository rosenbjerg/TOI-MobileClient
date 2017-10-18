using System;
using System.Collections.Generic;
using System.Text;
using TOIClasses;

namespace TOI_MobileClient.ViewModels
{
    class TagViewModel : ViewModelBase
    {
        private TagInfo _tagInfo;
        public string Title => _tagInfo.Title;
        public string ShortDescription => _tagInfo.Description;
        public string Image => _tagInfo.Image;
        public string Url => _tagInfo.Url;

        public TagViewModel(TagInfo tf)
        {
            _tagInfo = tf;
        }
    }
}
