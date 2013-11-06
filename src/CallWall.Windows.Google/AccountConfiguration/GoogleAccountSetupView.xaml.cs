using System.Windows.Controls;

namespace CallWall.Windows.Google.AccountConfiguration
{
    /// <summary>
    /// Interaction logic for GoogleAccountSetupView.xaml
    /// </summary>
    public partial class GoogleAccountSetupView : UserControl, IGoogleAccountSetupView
    {
        public GoogleAccountSetupView(IGoogleAccountSetupViewModel viewModel)
        {
            DataContext = ViewModel = viewModel;
            InitializeComponent();
        }

        public IGoogleAccountSetupViewModel ViewModel { get; private set; }
    }
}
