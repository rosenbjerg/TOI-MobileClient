using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.Models;
using TOI_MobileClient.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOI_MobileClient.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent ();
		}
	}

    public class SettingsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RadioTemplate { get; set; }
        public DataTemplate BooleanTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var type = ((SettingViewModel) item).Type;
            switch (type)
            {
                case Setting.SettingType.Boolean:
                    return BooleanTemplate;
                case Setting.SettingType.Radio:
                    return RadioTemplate;
                default:
                    return null;
            }
        }
    }
}