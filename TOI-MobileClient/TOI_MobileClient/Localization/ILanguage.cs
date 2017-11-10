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
        #endregion

        string ScanForTags { get; }
        string Settings { get; }
        string About { get; }

        string Scanning { get; }
        string ScanningExplanation { get; }
        string NewToi { get; }
        string NewToiExplanation { get; }

    }
}
