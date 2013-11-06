namespace CallWall.Windows.Connectivity.Settings.Bluetooth
{
    /// <summary>
    /// Interaction logic for BluetoothSetupView.xaml
    /// </summary>
    public partial class BluetoothSetupView : IBluetoothSetupView
    {
        private readonly IBluetoothSetupViewModel _viewModel;

        public BluetoothSetupView(IBluetoothSetupViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public IBluetoothSetupViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
