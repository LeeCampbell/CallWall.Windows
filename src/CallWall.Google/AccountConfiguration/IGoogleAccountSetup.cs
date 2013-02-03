using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CallWall.Google.AccountConfiguration
{
    public interface IGoogleAccountSetup : INotifyPropertyChanged
    {
        bool IsAuthorized { get; }
        ReadOnlyCollection<GoogleResource> Resources { get; }
        bool IsEnabled { get; set; }
        
        void Authorize();
    }
}