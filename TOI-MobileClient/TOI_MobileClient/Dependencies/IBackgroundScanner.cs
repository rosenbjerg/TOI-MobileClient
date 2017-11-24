using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.Dependencies
{
    public interface IBackgroundScanner
    {
        Task ScanForToi(HashSet<string> filter, ScanConfiguration configuration = null);

        event EventHandler<TagsFoundsEventArgs> TagsFound;
    }
    

    public class TagsFoundsEventArgs : EventArgs
    {
        public TagsFoundsEventArgs(List<string> addrs)
        {
            Tags = addrs;
        }

        public List<string> Tags { get; }
        public bool Handled { get; set; } = false;
    }

    public class ScanConfiguration
    {
        public bool UseBle;
        public bool UseNfc;
        public bool UseGps;
        public bool UseWifi;

        // TODO change values to true as implementation of each technology is completed.
        public ScanConfiguration(bool useBle = true, bool useNfc = true, bool useGps = true, bool useWifi = true)
        {
            UseBle = useBle;
            UseNfc = useNfc;
            UseGps = useGps;
            UseWifi = useWifi;
        }

        public static readonly ScanConfiguration DefaultScanConfiguration = new ScanConfiguration();
    }
}
