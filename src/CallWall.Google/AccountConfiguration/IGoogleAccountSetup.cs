using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CallWall.Google.AccountConfiguration
{
    public interface IGoogleAccountSetup : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<GoogleResource> Resources { get; }
        bool IsEnabled { get; set; }
        bool IsAuthorized { get; set; }
        
        void Authorize();
    }
}