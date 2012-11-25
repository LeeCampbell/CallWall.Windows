using System.Windows.Controls;

namespace CallWall.Settings.Connectivity
{
    /// <summary>
    /// Interaction logic for ConnectivitySettingsView.xaml
    /// </summary>
    public partial class ConnectivitySettingsView : UserControl, IConnectivitySettingsView
    {
        private readonly IConnectivitySettingsViewModel _viewModel;

        public ConnectivitySettingsView(IConnectivitySettingsViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
        }

        #region Implementation of IConnectivitySettingsView

        public IConnectivitySettingsViewModel ViewModel
        {
            get { return _viewModel; }
        }

        #endregion
    }
}
