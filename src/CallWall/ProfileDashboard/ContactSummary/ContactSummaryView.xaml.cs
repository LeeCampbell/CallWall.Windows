using System.Windows.Controls;

namespace CallWall.ProfileDashboard.ContactSummary
{
    /// <summary>
    /// Interaction logic for ContactSummaryView.xaml
    /// </summary>
    public partial class ContactSummaryView : UserControl
    {
        private readonly ContactSummaryViewModel _viewModel;

        public ContactSummaryView(ContactSummaryViewModel viewModel)
        {
            DataContext = _viewModel = viewModel;
            InitializeComponent();
        }
    }
}
