using System.Windows.Controls;

namespace CallWall.Settings.Providers
{
    /// <summary>
    /// Interaction logic for IProviderSettingsView.xaml
    /// </summary>
    public partial class ProviderSettingsView : UserControl, IProviderSettingsView
    {
        private readonly IProviderSettingsViewModel _viewModel;

        public ProviderSettingsView(IProviderSettingsViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
        }

        #region Implementation of IProviderSettingsView

        public IProviderSettingsViewModel ViewModel
        {
            get { return _viewModel; }
        }

        #endregion
    }
}
