using System;
using Xamarin.Forms;

namespace TOI_MobileClient
{

    public class MainPageMenuItem : MenuItem
    {
        public MainPageMenuItem()
        {
            TargetType = typeof(MainPageDetail);
        }
        public int MenuItemId { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}