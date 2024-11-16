using PotatoWPF.Models;
using PotatoWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace PotatoWPF.Templates
{
    public partial class TableTemplate : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Notify the UI when a property has changed
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Set property and notify UI only if the value changes
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
        public static readonly DependencyProperty MaxHeightOffsetProperty =
           DependencyProperty.Register(
               nameof(MaxHeightOffset),
               typeof(double),
               typeof(TableTemplate),
               new PropertyMetadata(465.0, OnMaxHeightOffsetChanged));
        public double MaxHeightOffset
        {
            get { return (double)GetValue(MaxHeightOffsetProperty); }
            set { SetValue(MaxHeightOffsetProperty, value); }
        }
        public List<PotatoModel> addedRowIds { get; } = new List<PotatoModel>();
        public List<string> editedRowIds { get; } = new List<string>();
        public List<string> deletedRowIds { get; } = new List<string>();
        public ICommand DeleteCommand { get; }
        private ObservableCollection<PotatoModel> _dataList;
        public ObservableCollection<PotatoModel> DataList
        {
            get => _dataList;
            set => SetProperty(ref _dataList, value);
        }
        public TableTemplate()
        {
            InitializeComponent();

            DataContext = this;

            // Set initial DataGrid MaxHeight on load
            this.Loaded += TableTemplate_Loaded;

            DeleteCommand = new RelayCommand(DeleteItem);

            // Adjust DataGrid MaxHeight if the UserControl is resized
            this.SizeChanged += TableTemplate_SizeChanged;
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
            if (calculatedMaxHeight > 100 && DataGrid != null)
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

        public DataGridTemplateColumn deleteTemplateColumn;
        public void SetColumns(List<DataGridColumn> columns)
        {
            deleteTemplateColumn = new DataGridTemplateColumn
            {
                Header = "",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                CellTemplate = CreateDeleteButtonTemplate()
            };

            columns.Add(deleteTemplateColumn);
            DataGrid.Columns.Clear();
            foreach (var column in columns)
            {
                DataGrid.Columns.Add(column);
            }
        }

        // Define public events
        public event RoutedEventHandler ApplyClicked;
        public event RoutedEventHandler CancelClicked;
        public event RoutedEventHandler DeleteClicked;
        public event RoutedEventHandler AddNewClicked;
        public event EventHandler<DataGridCellEditEndingEventArgs> EditDataGridCell;
        public event EventHandler<MouseButtonEventArgs> DataGridMouseLeftButtonUp;
        protected virtual void OnDataGridMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            DataGridMouseLeftButtonUp?.Invoke(this, e);
        }

        protected virtual void OnEditDataGridCell(DataGridCellEditEndingEventArgs e)
        {
            EditDataGridCell?.Invoke(this, e);
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

        protected virtual void OnDeleteClicked()
        {
            DeleteClicked?.Invoke(this, new RoutedEventArgs());
        }

        protected virtual void OnAddNewClicked()
        {
            AddNewClicked?.Invoke(this, new RoutedEventArgs());
        }

        private void OnDataGridNewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnDataGridMouseLeftButtonUp(e);
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

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            OnDeleteClicked();
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
            deleteButton.SetValue(Button.HeightProperty, 30.0);
            deleteButton.SetValue(Button.ContentProperty, "DELETE");
            deleteButton.SetBinding(Button.CommandProperty, new Binding("DeleteCommand")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(TableTemplate), 1)
            });
            deleteButton.SetBinding(Button.CommandParameterProperty, new Binding("."));

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
        private void DeleteItem(object parameter)
        {
            if (parameter is PotatoModel selectedItem)
            {
                addedRowIds.Remove(selectedItem);

                if (DataList.Contains(selectedItem))
                {
                    DataList.Remove(selectedItem);
                }
                deletedRowIds.Add(selectedItem.Id.ToString());
                DataGrid.Items.Refresh();  // Refresh the DataGrid
            }
        }


        // Title property
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TableTemplate), new PropertyMetadata("Title"));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // ImageSource property
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(TableTemplate), new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // CanvasContent property
        public static readonly DependencyProperty CanvasContentProperty =
            DependencyProperty.Register("CanvasContents", typeof(object), typeof(TableTemplate), new PropertyMetadata(null));

        public object CanvasContents
        {
            get { return GetValue(CanvasContentProperty); }
            set { SetValue(CanvasContentProperty, value); }
        }

        public TextBox CreateTextBox(string bindingProperty, double left, double top)
        {
            var textBox = new TextBox
            {
                Width = 80,
                Height = 25
            };

            // Bind the TextBox Text to the specified property (Dim1, Dim2, Dim3)
            textBox.SetBinding(TextBox.TextProperty, new Binding(bindingProperty)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            });

            // Set position of TextBox within the StackPanel
            Canvas.SetLeft(textBox, left);
            Canvas.SetTop(textBox, top);

            return textBox;
        }
        public void RefreshDataGrid()
        {
            editedRowIds.Clear();
            deletedRowIds.Clear();
            addedRowIds.Clear();
            ImageItems.Visibility = Visibility.Collapsed;
        }

    }
}
