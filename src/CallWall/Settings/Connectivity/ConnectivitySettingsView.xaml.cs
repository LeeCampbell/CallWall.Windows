namespace CallWall.Settings.Connectivity
{
    /// <summary>
    /// Interaction logic for ConnectivitySettingsView.xaml
    /// </summary>
    public partial class ConnectivitySettingsView : IConnectivitySettingsView
    {
        private readonly IConnectivitySettingsViewModel _viewModel;

        public ConnectivitySettingsView(IConnectivitySettingsViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public IConnectivitySettingsViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
