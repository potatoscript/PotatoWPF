using System.Windows;
using System.Windows.Controls;

namespace PotatoWPF.Views
{
    /// <summary>
    /// Interaction logic for PotatoView.xaml
    /// </summary>
    public partial class PotatoView : UserControl
    {
        public PotatoView()
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
                    case "Recipe1View":
                        content = new Recipe1View();
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
