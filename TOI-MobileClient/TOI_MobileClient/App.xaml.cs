using System;
using System.Collections.Generic;
using System.Linq;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using DepMan;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Localization;
using TOI_MobileClient.Managers;
using TOI_MobileClient.Views;
using Xamarin.Forms;

namespace TOI_MobileClient
{
	public partial class App
	{
	    public static INavigation Navigation;
		public App ()
		{
			InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
		    Navigation = MainPage.Navigation;
        }

	    protected override void OnStart()
	    {
	        if (DependencyManager.IsRegistered<RestClient>()) return;
	        DependencyManager.Register<RestClient, RestClient>(new RestClient(new ToiHttpManager()));
	        DependencyManager.Register<ILanguage, EnglishLanguage>(new EnglishLanguage());
	    }

	    protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}