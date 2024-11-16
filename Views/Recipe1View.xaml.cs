using PotatoWPF.Models;
using PotatoWPF.Templates;
using PotatoWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PotatoWPF.Views
{
    public partial class Recipe1View : UserControl
    {
        //Uri baseUri = (Uri)Application.Current.Resources["BaseImageUri"];

        private readonly Item1ViewModel viewModel;
        
        public List<PotatoModel> addedRowIds { get; } = new List<PotatoModel>();

        private Dictionary<string, DateTime> lastEditTimes = new Dictionary<string, DateTime>();

        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Update MaxHeightOffset whenever the grid size changes
            UpdateMaxHeightOffset();
        }

        private void UpdateMaxHeightOffset()
        {
            // Calculate the new MaxHeightOffset based on the Grid's ActualHeight
            double gridHeight = MainGrid.ActualHeight;
        }

        private readonly Dictionary<string, string> localizedStrings;
        private Dictionary<string, string> LoadLocalizedStrings()
        {
            return new Dictionary<string, string>
            {
                { "str_OnCancelClicked", (string)Application.Current.Resources["str_OnCancelClicked"] },
                { "str_OnApplyClicked", (string)Application.Current.Resources["str_OnApplyClicked"] },
                { "str_NoRowToChange", (string)Application.Current.Resources["str_NoRowToChange"] },
                { "str_title", (string)Application.Current.Resources["str_recipe01"] },
            };
        }

        public Recipe1View()
        {
            InitializeComponent();

            viewModel = new Item1ViewModel();
            DataContext = viewModel;

            SetColumns();
            SubscribeToEvents();
            

            // Load localized strings once
            localizedStrings = LoadLocalizedStrings();

            TableControl.TitleLabel.Content = "";

            RefreshDataGrid();
        }

        private void SetColumns()
        {
            // Create the DataTemplate for displaying images in the ComboBox
            var imageDataTemplate = new DataTemplate();
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var imageFactory = new FrameworkElementFactory(typeof(Image));
            imageFactory.SetBinding(Image.SourceProperty, new Binding()); // Bind the image source
            imageFactory.SetValue(Image.WidthProperty, 40.0);
            imageFactory.SetValue(Image.HeightProperty, 40.0);
            stackPanelFactory.AppendChild(imageFactory);

            imageDataTemplate.VisualTree = stackPanelFactory;

            var columns = new List<DataGridColumn>
            {
                new DataGridTextColumn
                {
                    Header = "ID",
                    Binding = new System.Windows.Data.Binding("Id"),
                    Visibility = Visibility.Collapsed
                },
                new DataGridTextColumn
                {
                    Header = "TITLE",
                    Binding = new System.Windows.Data.Binding("Title"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                },
                new DataGridTextColumn
                {
                    Header = "TYPE",
                    Binding = new System.Windows.Data.Binding("Type"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Auto)
                },
                new DataGridTextColumn
                {
                    Header = "VALUE",
                    Binding = new System.Windows.Data.Binding("Value"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Auto)
                },
                new DataGridComboBoxColumn
                {
                    Header = "Image",
                    ItemsSource = viewModel.PotatoOptions,
                    SelectedItemBinding = new System.Windows.Data.Binding("ImageSource"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = new Style(typeof(ComboBox))
                    {
                        Setters =
                        {
                            new Setter(ComboBox.ItemTemplateProperty, imageDataTemplate) // Set the ItemTemplate
                        }
                    },
                    EditingElementStyle = new Style(typeof(ComboBox))
                    {
                        Setters =
                        {
                            new Setter(ComboBox.ItemTemplateProperty, imageDataTemplate) // Set the ItemTemplate
                        }
                    }
                }
            };
            // Assign columns to the DataGrid
            TableControl.SetColumns(columns);
        }

        
        private void SubscribeToEvents()
        {
            TableControl.ApplyClicked += TableControl_ApplyClicked;
            TableControl.CancelClicked += TableControl_CancelClicked;
            TableControl.AddNewClicked += TableControl_AddNewClicked;
            TableControl.DeleteClicked += TableControl_DeleteClicked;
            
            TableControl.EditDataGridCell += TableControl_EditDataGridCell;
            //TableControl.DataGridMouseLeftButtonUp += TableControl_DataGridMouseLeftButtonUp;
            //Beam.RequestAction += RefreshDataGrid;
        }

        private void TableControl_EditDataGridCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is PotatoModel editedItem)
            {
                string itemId = editedItem.Id.ToString();
                DateTime currentTime = DateTime.Now;

                // Check if item has been edited recently
                if (!lastEditTimes.ContainsKey(itemId) || (currentTime - lastEditTimes[itemId]).TotalMilliseconds > 100)
                {
                    // Update the last edit time for this item
                    lastEditTimes[itemId] = currentTime;

                    // Add the ID to editedRowIds if not already present
                    if (!TableControl.editedRowIds.Contains(itemId))
                    {
                        TableControl.editedRowIds.Add(itemId);
                    }

                    // Force commit of the cell edit to ensure updated value is available
                    TableControl.DataGrid.CommitEdit(DataGridEditingUnit.Row, true);

                }
            }
        }

        private void TableControl_ApplyClicked(object sender, RoutedEventArgs e)
        {
            if (TableControl.editedRowIds.Count > 0 || TableControl.deletedRowIds.Count > 0 || addedRowIds.Count > 0)
            {
                // Apply changes to the SQLite database
                viewModel.ApplyChanges(TableControl.editedRowIds, TableControl.deletedRowIds, addedRowIds);

                // Show confirmation that the data has been updated
                MessageBox.Show(localizedStrings["str_OnApplyClicked"], 
                    "Updated", MessageBoxButton.OK, MessageBoxImage.Information);

                RefreshDataGrid();
                
            }
            else
            {
                MessageBox.Show(localizedStrings["str_NoRowToChange"], "Comfirm", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TableControl_CancelClicked(object sender, RoutedEventArgs e)
        {
            // Confirm cancellation
            MessageBoxResult result = MessageBox.Show(
                localizedStrings["str_OnCancelClicked"],
                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {

                RefreshDataGrid();
            }
        }

        private void TableControl_AddNewClicked(object sender, RoutedEventArgs e)
        {
            var newRow = new PotatoModel
            {
                Title = "New Title",
                Type = "NewType",
                Value = 0,
                Deleteable = 1
            };
            addedRowIds.Add(newRow);
            viewModel.DataList.Add(newRow);
        }

        private void TableControl_DeleteClicked(object sender, RoutedEventArgs e)
        {
            if (sender is PotatoModel selectedItem)
            {
                addedRowIds.Remove(selectedItem); // Remove the selected item
                //RemoveDataFromJson(selectedItem.ID);
                // Remove the selected item from newDataItems if it exists there
                if (viewModel.DataList.Contains(selectedItem))
                {
                    viewModel.DataList.Remove(selectedItem);
                }
                TableControl.deletedRowIds.Add(selectedItem.Id.ToString());
                TableControl.DataGrid.Items.Refresh();  // Refresh the DataGrid
            }
        }

        private void RefreshDataGrid()
        {
            viewModel.DataList.Clear();
            viewModel.LoadAllData();
            TableControl.DataGrid.ItemsSource = viewModel.DataList;
            // Clear lists after applying changes
            TableControl.editedRowIds.Clear();
            TableControl.deletedRowIds.Clear();
            addedRowIds.Clear();

        }
    }
}
