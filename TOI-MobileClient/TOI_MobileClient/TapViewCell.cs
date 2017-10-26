using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TOI_MobileClient
{
    class TapViewCell : ViewCell
    {
        public ICommand Command { get; set; }

        public TapViewCell()
        {
            Tapped += (sender, args) =>
            {
                if (Command?.CanExecute(null) ?? false)
                    Command.Execute(null);
            };
        }
    }
}
