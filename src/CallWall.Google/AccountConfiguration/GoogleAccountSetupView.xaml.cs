using System.Windows.Controls;

namespace CallWall.Google.AccountConfiguration
{
    /// <summary>
    /// Interaction logic for GoogleAccountSetupView.xaml
    /// </summary>
    public partial class GoogleAccountSetupView : UserControl, IGoogleAccountSetupView
    {
        public GoogleAccountSetupView(GoogleAccountSetupViewModel viewModel)
        {
            DataContext = ViewModel = viewModel;
            InitializeComponent();
        }

        public GoogleAccountSetupViewModel ViewModel { get; private set; }
    }
}
