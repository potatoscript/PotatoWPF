using PotatoWPF.ViewModels;
using PotatoWPF.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PotatoWPF.Templates
{
    /// <summary>
    /// TableTemplate.xaml の相互作用ロジック
    /// </summary>
    public partial class TableTemplate : UserControl
    {
        public static readonly DependencyProperty MaxHeightOffsetProperty =
           DependencyProperty.Register(
               nameof(MaxHeightOffset),
               typeof(double),
               typeof(TableTemplate),
               new PropertyMetadata(300.0, OnMaxHeightOffsetChanged));

        public double MaxHeightOffset
        {
            get { return (double)GetValue(MaxHeightOffsetProperty); }
            set { SetValue(MaxHeightOffsetProperty, value); }
        }


        public List<string> editedRowIds { get; } = new List<string>();
        public List<string> deletedRowIds { get; } = new List<string>();

        public ICommand DeleteCommand { get; }

        public DataGridTemplateColumn deleteTemplateColumn;

        public TableTemplate()
        {
            InitializeComponent();

            deleteTemplateColumn = new DataGridTemplateColumn
            {
                Header = "",
                Width = DataGridLength.Auto,
                CellTemplate = CreateDeleteButtonTemplate()
            };


        // Set initial DataGrid MaxHeight on load
        this.Loaded += TableTemplate_Loaded;

            // Adjust DataGrid MaxHeight if the UserControl is resized
            this.SizeChanged += TableTemplate_SizeChanged;

            DeleteCommand = new RelayCommand(DeleteItem);

        }


        private DataTemplate CreateDeleteButtonTemplate()
        {
            var cellTemplate = new DataTemplate();

            // StackPanel to hold delete button
            var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanel.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Right);

            // Delete button definition
            var deleteButton = new FrameworkElementFactory(typeof(Button));
            deleteButton.SetValue(Button.ContentProperty, "DELETE");
            deleteButton.SetBinding(Button.CommandProperty, new Binding("DeleteCommand")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Item1View), 1)
            });
            deleteButton.SetBinding(Button.CommandParameterProperty, new Binding("."));

            // Set the button style to the CustomButtonStyle defined in your ResourceDictionary
            deleteButton.SetValue(Button.StyleProperty, Application.Current.Resources["DeleteButtonStyle"]);


            stackPanel.AppendChild(deleteButton);
            cellTemplate.VisualTree = stackPanel;

            // Triggers to control button visibility based on Deleteable property
            cellTemplate.Triggers.Add(CreateDeleteButtonTrigger(0, Visibility.Collapsed));
            cellTemplate.Triggers.Add(CreateDeleteButtonTrigger(1, Visibility.Visible));

            return cellTemplate;
        }

        // Helper method to create a DataTrigger for delete button visibility
        private DataTrigger CreateDeleteButtonTrigger(int deleteableValue, Visibility visibility)
        {
            return new DataTrigger
            {
                Binding = new Binding("Deleteable"),
                Value = deleteableValue,
                Setters = { new Setter(Button.VisibilityProperty, visibility) }
            };
        }


        private void TableTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            SetDataGridMaxHeight();
        }

        private void TableTemplate_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetDataGridMaxHeight();
        }

        private void SetDataGridMaxHeight()
        {
            // Ensure we have the actual height of the TableTemplate
            double templateHeight = this.ActualHeight;

            // Calculate the DataGrid's MaxHeight based on MaxHeightOffset
            double calculatedMaxHeight = templateHeight - MaxHeightOffset;

            // Set the MaxHeight if the calculated value is valid
            if (calculatedMaxHeight > 0 && DataGrid != null)
            {
                DataGrid.MaxHeight = calculatedMaxHeight;
            }
            else if (DataGrid != null)
            {
                DataGrid.MaxHeight = 0; // Avoid negative MaxHeight
            }
        }

        private static void OnMaxHeightOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TableTemplate)d;
            control.SetDataGridMaxHeight(); // Update MaxHeight when MaxHeightOffset changes
        }


        public void SetColumns(List<DataGridColumn> columns)
        {
            DataGrid.Columns.Clear();
            foreach (var column in columns)
            {
                DataGrid.Columns.Add(column);
            }
        }

        // Define public events
        public event RoutedEventHandler ApplyClicked;
        public event RoutedEventHandler CancelClicked;
        public event RoutedEventHandler AddNewClicked;
        public event RoutedEventHandler DeleteClicked;
        public event EventHandler<DataGridCellEditEndingEventArgs> EditDataGridCell;

        protected virtual void OnEditDataGridCell(DataGridCellEditEndingEventArgs e)
        {
            EditDataGridCell?.Invoke(this, e);
        }

        public void DeleteItem(object obj)
        {
            var args = new RoutedEventArgs();
            DeleteClicked?.Invoke(this, args);
        }

        // Method to invoke the ApplyClicked event
        protected virtual void OnApplyClicked()
        {
            ApplyClicked?.Invoke(this, new RoutedEventArgs());
        }

        // Method to invoke the CancelClicked event
        protected virtual void OnCancelClicked()
        {
            CancelClicked?.Invoke(this, new RoutedEventArgs());
        }

        protected virtual void OnAddNewClicked()
        {
            AddNewClicked?.Invoke(this, new RoutedEventArgs());
        }

        // Handle the CellEditEnding event
        private void OnEditNewDataGridCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Pass the event arguments to the handler
            OnEditDataGridCell(e);
        }
        // Hook up the click events
        private void OnAddNewButtonClicked(object sender, RoutedEventArgs e)
        {
            OnAddNewClicked();
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            OnApplyClicked();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            OnCancelClicked();
        }


    }
}
