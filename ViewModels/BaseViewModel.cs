using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;

namespace PotatoWPF.ViewModels
{
    public class HeightToMaxDataGridHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double windowHeight)
            {
                double desiredHeight = windowHeight - 300; // Subtract 100px from window height
                return Math.Max(desiredHeight, 0); // Ensure height doesn't go below 0
            }
            return 500; // Fallback value if conversion fails
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BaseViewModel : INotifyPropertyChanged
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
    }

    // RelayCommand class for handling commands
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        // Constructor for initializing execute and canExecute functions
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Determines whether the command can execute
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        // Executes the command
        public void Execute(object parameter) => _execute(parameter);

        // Raise CanExecuteChanged when needed
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Method to manually raise CanExecuteChanged if necessary
        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }

    /// <summary>
    /// Used to set the visibility of x:Name="RightSection" in Separeta
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }
}
