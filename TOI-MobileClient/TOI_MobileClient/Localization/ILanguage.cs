using System;
using System.Collections.Generic;
using System.Text;

namespace TOI_MobileClient
{
    public interface ILanguage
    {
        #region ScanPage
        string BluetoothNotEnabled { get; }
        string NoNearbyTags { get; }
        string WifiNotEnabled { get; }
        string GpsNotEnabled { get; }
        string NfcNotEnabled { get; }
        #endregion

        #region Settings
        string ScanFrequency { get; }
        string Often { get; }
        string Normal { get; }
        string Rarely { get; }
        string Never { get; }

        string Gps { get; }
        string Bluetooth { get; }
        string Wifi { get; }
        string Nfc { get; }

        string EnableToast { get; }
        #endregion
        string NoContexts { get; }
        string ScanForTags { get; }
        string Settings { get; }
        string Contexts { get; }
        string About { get; }

        string Scanning { get; }
        string ScanningPaused { get; }
        string ScanningExplanation { get; }
        string NotScanningExplanation { get; }
        string NewToi { get; }
        string NewToiExplanation { get; }

    }
}
