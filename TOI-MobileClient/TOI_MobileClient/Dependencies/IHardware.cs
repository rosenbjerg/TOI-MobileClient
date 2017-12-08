using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    public interface IHardware
    {
        bool IsEnabled { get; }
    }

    public interface IScanner<TScanResult>
        where TScanResult : IScanResultEvent
    {
        Task ScanAsync();
        event EventHandler<TScanResult> ResultFound;
    }

    public interface IScanResultEvent
    {
        string Id { get; }
    }
}
