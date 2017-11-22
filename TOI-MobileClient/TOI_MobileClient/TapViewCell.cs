using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Type = Android.Renderscripts.Type;

namespace TOI_MobileClient
{
    public class TapViewCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TapViewCell));

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

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
