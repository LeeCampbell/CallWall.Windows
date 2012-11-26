using System;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;

namespace CallWall.Welcome
{
    /// <summary>
    /// Interaction logic for WelcomeStep1.xaml
    /// </summary>
    public partial class WelcomeStep1View : UserControl, IWelcomeStep1View
    {
        private readonly DelegateCommand _nextStepCommand;

        public WelcomeStep1View()
        {
            _nextStepCommand = new DelegateCommand(OnNextView);
            DataContext = this;
            InitializeComponent();
        }

        public DelegateCommand NextStepCommand
        {
            get { return _nextStepCommand; }
        }

        public event EventHandler NextView;

        private void OnNextView()
        {
            var handler = NextView;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
