using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.Dependencies
{
    public interface IBackgroundScanner
    {
        Task StartScan();

        event EventHandler<TagsFoundsEventArgs> TagsFound;
        event EventHandler<TagFoundEventArgs> TagFound;
    }

    public class TagFoundEventArgs : EventArgs
    {
        public string Tag { get; }
        public bool Gps { get; }

        public TagFoundEventArgs(string tag, bool gps = false)
        {
            Tag = tag;
            Gps = gps;
        }
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

}
