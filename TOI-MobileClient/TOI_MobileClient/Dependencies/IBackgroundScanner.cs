using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOIClasses;
using TOI_MobileClient.Managers;

namespace TOI_MobileClient.Dependencies
{
    public interface IBackgroundScanner
    {
        void StopBackgroundScanning();
        void StartBackgroundScanning();

        event EventHandler<ToisFoundEventArgs> ToisFound;
    }

    public class ToisFoundEventArgs : EventArgs
    {
        public List<ToiModel> Tois { get; }

        public ToisFoundEventArgs(List<ToiModel> tois)
        {
            Tois = tois;
        }
    }
}
