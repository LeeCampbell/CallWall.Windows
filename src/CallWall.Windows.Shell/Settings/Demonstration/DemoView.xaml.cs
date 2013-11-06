using System.Windows.Controls;

namespace CallWall.Windows.Shell.Settings.Demonstration
{
    public partial class DemoView : UserControl, IDemoView
    {
        private readonly DemoViewModel _viewModel;

        public DemoView(DemoViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public DemoViewModel ViewModel
        {
            get { return _viewModel; }
        }
    }
}
