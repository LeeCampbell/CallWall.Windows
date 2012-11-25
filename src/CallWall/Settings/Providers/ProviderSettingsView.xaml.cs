using System.Windows.Controls;

namespace CallWall.Settings.Providers
{
    /// <summary>
    /// Interaction logic for IProviderSettingsView.xaml
    /// </summary>
    public partial class ProviderSettingsView : UserControl, IProviderSettingsView
    {
        private ProviderSettingsViewModel _viewModel;

        public ProviderSettingsView(ProviderSettingsViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
        }

        #region Implementation of IProviderSettingsView

        public ProviderSettingsViewModel ViewModel
        {
            get { return _viewModel; }
        }

        #endregion
    }
}
