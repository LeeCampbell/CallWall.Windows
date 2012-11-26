namespace CallWall.Settings.Providers
{
    /// <summary>
    /// Interaction logic for IProviderSettingsView.xaml
    /// </summary>
    public partial class ProviderSettingsView : IProviderSettingsView
    {
        private readonly IProviderSettingsViewModel _viewModel;

        public ProviderSettingsView(IProviderSettingsViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public IProviderSettingsViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
