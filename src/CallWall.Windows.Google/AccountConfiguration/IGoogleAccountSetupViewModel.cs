using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Windows.Google.AccountConfiguration
{
    public interface IGoogleAccountSetupViewModel : INotifyPropertyChanged
    {
        bool IsAuthorized { get; }
        bool IsEnabled { get; set; }
        bool IsProcessing { get; }
        DelegateCommand AuthorizeCommand { get; }
        ReadOnlyCollection<GoogleResource> Resources { get; }
        ObservableCollection<GoogleResource> SelectedResources { get; }
    }
}