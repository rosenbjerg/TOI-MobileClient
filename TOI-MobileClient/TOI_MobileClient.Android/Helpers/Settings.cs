using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace TOI_MobileClient.Droid.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>        
    public static class Settings
    {
        private CrossSettings _appSettings;

	    static Settings()
	    {
	        _appSettings = CrossSettings.Current;
	    }

		public static bool BluetoothEnabled
		{
			get => AppSettings.GetValueOrDefault(nameof(BluetoothEnabled), true);
			set => AppSettings.AddOrUpdateValue(nameof(BluetoothEnabled), value);
		}

	    public static bool GpsEnabled
	    {
	        get => AppSettings.GetValueOrDefault(nameof(GpsEnabled), true);
	        set => AppSettings.AddOrUpdateValue(nameof(GpsEnabled), value);
	    }
	}
}