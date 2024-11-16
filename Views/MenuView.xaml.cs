using System.Windows;
using System.Windows.Controls;

namespace PotatoWPF.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }
        private void Recipe01_Click(object sender, RoutedEventArgs e)
        {
            var content = (UserControl)null;
            content = new Recipe1View();
            this.Content = content;
        }
    }

    

}
