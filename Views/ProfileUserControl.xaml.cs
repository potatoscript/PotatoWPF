using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PotatoWPF.Views
{
    /// <summary>
    /// Interaction logic for ProfileUserControl.xaml
    /// </summary>
    public partial class ProfileUserControl : UserControl
    {
        public ProfileUserControl()
        {
            InitializeComponent();
        }

        private void MenuTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue as TreeViewItem;
            if (selectedItem != null)
            {
                string tag = selectedItem.Tag as string;

                var content = (UserControl)null;
                // Load the corresponding content based on the selected tag
                switch (tag)
                {
                    case "Item1View":
                        content = new Item1View();
                        // /.;/./((ITitleText)content).TitleText = (string)Application.Current.Resources["str_kiso"];
                        break;
                    default:
                        content = null;
                        break;
                }

                MainContentControl.Content = content;

            }
        }
    }
}
