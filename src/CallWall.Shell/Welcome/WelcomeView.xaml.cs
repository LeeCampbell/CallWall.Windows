namespace CallWall.Welcome
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : IWelcomeView
    {
        private readonly WelcomeViewModel _viewModel;

        public WelcomeView(WelcomeViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public WelcomeViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
