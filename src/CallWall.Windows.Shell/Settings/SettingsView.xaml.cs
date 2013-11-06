namespace CallWall.Windows.Shell.Settings
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class SettingsView : ISettingsView
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsView(SettingsViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public SettingsViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
