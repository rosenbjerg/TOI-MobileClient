using System.Threading.Tasks;

namespace TOI_MobileClient.Dependencies
{
    interface IScannerServiceProvider
    {
        Task<IBackgroundScanner> GetServiceAsync();
    }
}