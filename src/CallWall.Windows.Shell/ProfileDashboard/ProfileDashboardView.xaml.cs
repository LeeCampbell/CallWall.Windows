namespace CallWall.Windows.Shell.ProfileDashboard
{
    /// <summary>
    /// Interaction logic for ProfileDashboardView.xaml
    /// </summary>
    public partial class ProfileDashboardView : IProfileDashboardView
    {
        private readonly ProfileDashBoardViewModel _viewModel;

        public ProfileDashboardView(ProfileDashBoardViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public ProfileDashBoardViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}