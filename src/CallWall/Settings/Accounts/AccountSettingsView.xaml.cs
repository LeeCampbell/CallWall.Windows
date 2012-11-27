namespace CallWall.Settings.Accounts
{
    /// <summary>
    /// Interaction logic for IAccountSettingsView.xaml
    /// </summary>
    public partial class AccountSettingsView : IAccountSettingsView
    {
        private readonly IAccountSettingsViewModel _viewModel;

        public AccountSettingsView(IAccountSettingsViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public IAccountSettingsViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
