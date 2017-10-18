using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    public class FontAwesomeIcon : Label
    {
        //Must match the exact "Name" of the font which you can get by double clicking the TTF in Windows
        public const string Typeface = "FontAwesome";

        public FontAwesomeIcon()
        {
            FontFamily = Typeface;    //iOS is happy with this, Android needs a renderer to add ".ttf"
        }
    }
}
