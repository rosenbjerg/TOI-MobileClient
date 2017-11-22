using System;
using System.Collections.Generic;
using System.Text;

namespace TOI_MobileClient.Localization
{
    public class EnglishLanguage : ILanguage
    {
        public string BluetoothNotEnabled => "Bluetooth is not enabled";
        public string NoNearbyTags => "No nearby ToI";
        public string ScanFrequency => "Scan Frequency";
        public string Often => "Often";
        public string Normal => "Normal";
        public string Rarely => "Rarely";
        public string Never => "Never";
        public string Gps => "GPS";
        public string Bluetooth => "Bluetooth";
        public string Wifi => "Wi-Fi";
        public string Nfc => "NFC";
        public string ScanForTags => "Scan for ToI";
        public string Settings => "Settings";
        public string About => "About";
        public string EnableToast => "Please enable ";
        public string Scanning => "Scanning...";
        public string ScanningExplanation => "Scanning in the background to find things of interest near you.";

        public string NewToi => "ToI near you";

        public string NewToiExplanation =>
            "There is something nearby that you might be interested in learning more about.";
    }
}
