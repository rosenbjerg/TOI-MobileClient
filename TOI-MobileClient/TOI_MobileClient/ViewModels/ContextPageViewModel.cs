using System.Windows.Input;
using Xamarin.Forms;

namespace TOI_MobileClient.ViewModels
{
    public class ContextPageViewModel : ContextPageViewModelBase
    {
        
        public ICommand SaveContext { get; }

        public ContextPageViewModel()
        {
            SaveContext = new Command(SaveContexts);
        }

        public override void SaveContexts()
        {
            
        }
    }
}