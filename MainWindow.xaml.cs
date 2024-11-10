using PotatoWPF.Views;
using System.Windows;

namespace PotatoWPF
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

        private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Clear current content
            MainContentGrid.Children.Clear();

            // Create an instance of the ProfileUserControl
            ProfileUserControl profileControl = new ProfileUserControl();

            // Set horizontal and vertical alignment to Top and Left
            profileControl.HorizontalAlignment = HorizontalAlignment.Left;
            profileControl.VerticalAlignment = VerticalAlignment.Top;

            // Add the ProfileUserControl to the MainContentGrid
            MainContentGrid.Children.Add(profileControl);
        }
    }
}
