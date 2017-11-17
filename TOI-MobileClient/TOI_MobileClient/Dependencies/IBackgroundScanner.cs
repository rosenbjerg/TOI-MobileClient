using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public interface IBackgroundScanner
    {
        void ScanForToi(HashSet<string> filter);

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
}
