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
            MainView();
        }
        private void MainView()
        {
            MainContentGrid.Children.Clear();
            MenuView home = new MenuView();
            MainContentGrid.Children.Add(home);
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainView();
        }
        private void PotatoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Clear current content
            MainContentGrid.Children.Clear();

            // Create an instance of the PotatoView
            PotatoView profileControl = new PotatoView();

            // Set horizontal and vertical alignment to Top and Left
            profileControl.HorizontalAlignment = HorizontalAlignment.Left;
            profileControl.VerticalAlignment = VerticalAlignment.Top;

            // Add the PotatoView to the MainContentGrid
            MainContentGrid.Children.Add(profileControl);
        }
    }
}
