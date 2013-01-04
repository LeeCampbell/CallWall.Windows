namespace CallWall.Settings.Connectivity.Bluetooth
{
    /// <summary>
    /// Interaction logic for BluetoothSetupView.xaml
    /// </summary>
    public partial class BluetoothSetupView : IBluetoothSetupView
    {
        private readonly BluetoothSetupViewModel _viewModel;

        public BluetoothSetupView(BluetoothSetupViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public BluetoothSetupViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
