using System;

namespace CallWall.Windows.Google.Authorization.Login
{
    /// <summary>
    /// Interaction logic for GoogleLoginView.xaml
    /// </summary>
    public partial class GoogleLoginView : IGoogleLoginView
    {
        private readonly GoogleLoginViewModel _viewModel;
        private bool _isActive;

        public GoogleLoginView()
        {
            InitializeComponent();
        }

        public GoogleLoginView(GoogleLoginViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }

        public GoogleLoginViewModel ViewModel
        {
            get { return _viewModel; }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        public event EventHandler IsActiveChanged;
        private void OnIsActiveChanged()
        {
            var handler = IsActiveChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
