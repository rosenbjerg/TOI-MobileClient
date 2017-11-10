using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public interface IBackgroundScanner
    {
        void ScanForToi(HashSet<Guid> filter);

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
}
