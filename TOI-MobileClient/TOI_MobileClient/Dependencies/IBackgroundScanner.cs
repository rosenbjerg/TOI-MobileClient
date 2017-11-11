using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.Dependencies
{
    public interface IBackgroundScanner
    {
        void ScanForToi(HashSet<Guid> filter, ScanConfiguration configuration = null);

        event EventHandler<TagsFoundsEventArgs> TagsFound;
    }
    

    public class TagsFoundsEventArgs : EventArgs
    {
        public TagsFoundsEventArgs(List<Guid> addrs)
        {
            Tags = addrs;
        }

        public List<Guid> Tags { get; }
        public bool Handled { get; set; } = false;
    }

    public class ScanConfiguration
    {
        public bool UseBle;
        public bool UseNfc;
        public bool UseGps;
        public bool UseWifi;

        public ScanConfiguration(bool useBle = true, bool useNfc = false, bool useGps = false, bool useWifi = false)
        {
            UseBle = useBle;
            UseNfc = useNfc;
            UseGps = useGps;
            UseWifi = useWifi;
        }

        public static readonly ScanConfiguration DefaultScanConfiguration = new ScanConfiguration();
    }
}
