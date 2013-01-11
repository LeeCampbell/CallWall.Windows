using System.Windows;

namespace CallWall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
