using PotatoWPF.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;

namespace PotatoWPF.Data
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageName = value as string;
            if (!string.IsNullOrEmpty(imageName))
            {
                return new BitmapImage(new Uri($"/Resources/Images/{imageName}.jpg", UriKind.Relative));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class DivisionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure there are 3 values: length, factor, and divisor
            if (values.Length >= 2 && values[0] is double length && values[1] is double factor)
            {
                // Use parameter as divisor if it's valid; otherwise, default to 1
                if (parameter is string paramStr && double.TryParse(paramStr, out double divisor) && divisor != 0)
                {
                    return (length * factor) / divisor;
                }
                else
                {
                    return (length * factor);
                }
            }

            // Return a default if the values or divisor are invalid
            return "Invalid value";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DivisionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the value is a double (lengthMax) and the parameter is provided
            if (value is double lengthMax && parameter != null)
            {
                // Default values
                double divisor = 1; // Default divisor to 1
                double multiplier = 1; // Default multiplier to 1

                // Try to parse the parameter as a comma-separated string
                var parameters = parameter.ToString().Split(',');

                // If both multiplier and divisor are provided, assign them
                if (parameters.Length > 1)
                {
                    // Assign divisor and multiplier
                    double.TryParse(parameters[0], out divisor);
                    double.TryParse(parameters[1], out multiplier);
                }
                else if (parameters.Length == 1)
                {
                    // If only one parameter is provided, assume it's the divisor
                    double.TryParse(parameters[0], out divisor);
                }

                // Perform the calculation: lengthMax * multiplier / divisor
                double result = (lengthMax * multiplier) / divisor;
                return result.ToString("0.00"); // Format to 2 decimal places
            }

            // Return a default if the value or parameter is invalid
            return "Invalid value";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




    public class MessageBoxHelper
    {
        private readonly Dictionary<string, string> localizedStrings;

        // Constructor to load localized strings
        public MessageBoxHelper()
        {
            localizedStrings = LoadLocalizedStrings();
        }

        // Method to load localized strings from resources
        private Dictionary<string, string> LoadLocalizedStrings()
        {
            return new Dictionary<string, string>
            {
                { "str_Kakunin", (string)Application.Current.Resources["str_kakunin"] },
                { "str_OnCancelClicked", (string)Application.Current.Resources["str_OnCancelClicked"] },
                { "str_Kousinkanryou", (string)Application.Current.Resources["str_kousinkanryou"] },
                { "str_OnApplyClicked", (string)Application.Current.Resources["str_OnApplyClicked"] },
                { "str_NoRowToChange", (string)Application.Current.Resources["str_NoRowToChange"] }
            };
        }

        // Method for showing a simple information message box
        public void ShowInfo(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Method for showing a warning message box
        public void ShowWarning(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        // Method for showing a yes/no confirmation message box
        public MessageBoxResult ShowConfirmation(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }

        public void ShowSuccessUpdated()
        {
            ShowInfo(localizedStrings["str_OnApplyClicked"], localizedStrings["str_Kousinkanryou"]);
        }

        public void ShowNoRowToChange()
        {
            ShowWarning(localizedStrings["str_NoRowToChange"], localizedStrings["str_Kakunin"]);
        }

        public MessageBoxResult ShowOnCancelClicked()
        {
            return MessageBox.Show(localizedStrings["str_OnCancelClicked"],
                                   localizedStrings["str_Kakunin"],
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Warning);
        }
    }


    public class DBHelper
    {
        private readonly string _connectionString;
        public string DbFilePath { get; }
        public SQLiteConnection Connection { get; private set; }
        private Dictionary<string, string> resources;
        Uri BaseImageUri { get; }

        // Constructor
        public DBHelper()
        {
            string dbDirectory = Application.Current.Resources["dbDirectory"] as string;
            string dbName = Application.Current.Resources["dbName"] as string;
            DbFilePath = Path.Combine(dbDirectory, dbName);
            BaseImageUri = (Uri)Application.Current.Resources["BaseImageUri"];
            _connectionString = $"Data Source={DbFilePath};Version=3;";

            InitializeResources();
            InitializeDatabase();
        }

        private void InitializeResources()
        {
            resources = new Dictionary<string, string>
            {
                { "str_recipe01", (string)Application.Current.Resources["str_recipe01"] },
                { "icon_potato", (string)Application.Current.Resources["icon_potato"] },
            };
        }
        private void InitializeDatabase()
        {
            if (!File.Exists(DbFilePath))
            {
                SQLiteConnection.CreateFile(DbFilePath);
            }

            using (Connection = new SQLiteConnection(_connectionString))
            {
                Connection.Open();
                CreateTables(Connection);
                CreateData();
            }
        }
        private void ExecuteNonQuery(SQLiteConnection Connection, string query)
        {
            using (var command = new SQLiteCommand(query, Connection))
            {
                command.ExecuteNonQuery();
            }
        }
        // Create necessary tables
        private void CreateTables(SQLiteConnection connection)
        {
            var tableDefinitions = new Dictionary<string, string>
            {
                {"PotatoDBTable", @"
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        TITLE TEXT,
                        TYPE TEXT,
                        VALUE DOUBLE,
                        ImageSource TEXT,
                        Deleteable INTEGER
                    )"
                },
            };
            foreach (var table in tableDefinitions)
            {
                string command = $"CREATE TABLE IF NOT EXISTS {table.Key} {table.Value};";
                ExecuteNonQuery(connection, command);
            }
        }
        private void CreateData()
        {
            InitializeResources();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                InsertInitialDataIfEmpty(connection, "PotatoDBTable", GetPotatoData());
            }
        }
        private void InsertInitialDataIfEmpty(SQLiteConnection connection, string tableName, IEnumerable<IDictionary<string, object>> data)
        {
            if (IsTableEmpty(connection,tableName))
            {
                InsertData(tableName, data);
            }
        }

        private bool IsTableEmpty(SQLiteConnection connection, string tableName)
        {
            using (var command = new SQLiteCommand($"SELECT COUNT(*) FROM {tableName};", connection))
            {
                long count = (long)command.ExecuteScalar();
                return count == 0;
            }
        }
        public void InsertData(string tableName, IEnumerable<IDictionary<string, object>> data)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                if (!data.Any()) return;

                var keys = string.Join(", ", data.First().Keys);
                var parameters = string.Join(", ", data.First().Keys.Select(k => $"@{k}"));
                string insertQuery = $"INSERT INTO {tableName} ({keys}) VALUES ({parameters});";

                using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                {
                    foreach (var item in data)
                    {
                        insertCommand.Parameters.Clear();
                        foreach (var kvp in item)
                        {
                            insertCommand.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        }
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        public void LoadAllData(string tableName, Dictionary<string, string> columnMappings, ObservableCollection<PotatoModel> DataList)
        {
            var data = ReadAllData(tableName);
            DataList.Clear();

            foreach (var row in data)
            {
                var item = new PotatoModel();

                foreach (var mapping in columnMappings)
                {
                    // mapping.Key is the property name, mapping.Value is the column name
                    var propertyInfo = item.GetType().GetProperty(mapping.Key);
                    if (propertyInfo != null && row.ContainsKey(mapping.Value))
                    {
                        var value = row[mapping.Value];
                        if (value != null && value != DBNull.Value)
                        {
                            // Convert the value to the property's type and assign it
                            propertyInfo.SetValue(item, Convert.ChangeType(value, propertyInfo.PropertyType));
                        }

                    }
                    else if (mapping.Value == "～")
                    {
                        propertyInfo.SetValue(item, mapping.Value);
                    }
                }

                DataList.Add(item);
            }
        }
        public void LoadData(string tableName, Dictionary<string, string> columnMappings, Dictionary<string, object> conditions, ObservableCollection<PotatoModel> DataList)
        {
            var data = ReadData(tableName, conditions);
            DataList.Clear();

            foreach (var row in data)
            {
                var item = new PotatoModel();

                foreach (var mapping in columnMappings)
                {
                    // mapping.Key is the property name, mapping.Value is the column name
                    var propertyInfo = item.GetType().GetProperty(mapping.Key);
                    if (propertyInfo != null && row.ContainsKey(mapping.Value))
                    {
                        var value = row[mapping.Value];
                        if (value != null && value != DBNull.Value)
                        {
                            // Convert the value to the property's type and assign it
                            propertyInfo.SetValue(item, Convert.ChangeType(value, propertyInfo.PropertyType));
                        }

                    }
                    else if (mapping.Value == "～")
                    {
                        propertyInfo.SetValue(item, mapping.Value);
                    }
                }

                DataList.Add(item);
            }
        }

        // Read data
        public IEnumerable<Dictionary<string, object>> ReadAllData(string tableName)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT * FROM {tableName} ORDER BY Id;";
                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                }
            }
            return result;
        }
        public IEnumerable<Dictionary<string, object>> ReadData(string tableName, Dictionary<string, object> conditions)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Start with the basic query structure
                string selectQuery = $"SELECT * FROM {tableName} WHERE 1=1";  // Always true condition for appending further conditions

                // Dynamically add conditions to the WHERE clause
                foreach (var condition in conditions)
                {
                    selectQuery += $" AND {condition.Key} = @{condition.Key}";
                }

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    // Add parameters to the command based on the conditions dictionary
                    foreach (var condition in conditions)
                    {
                        command.Parameters.AddWithValue($"@{condition.Key}", condition.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }
        // Update data
        public void UpdateData(string tableName, Dictionary<string, object> updatedValues, Dictionary<string, object> conditions)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var setClause = string.Join(", ", updatedValues.Keys.Select(k => $"{k} = @{k}"));
                var whereClause = string.Join(" AND ", conditions.Keys.Select(k => $"{k} = @condition_{k}"));
                string updateQuery = $"UPDATE {tableName} SET {setClause} WHERE {whereClause};";

                using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    foreach (var kvp in updatedValues)
                    {
                        updateCommand.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                    }
                    foreach (var kvp in conditions)
                    {
                        updateCommand.Parameters.AddWithValue($"@condition_{kvp.Key}", kvp.Value);
                    }
                    updateCommand.ExecuteNonQuery();
                }
            }
        }
        public void UpdateAllData(string tableName, Dictionary<string, object> updatedValues)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var setClause = string.Join(", ", updatedValues.Keys.Select(k => $"{k} = @{k}"));
                string updateQuery = $"UPDATE {tableName} SET {setClause};";

                using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    foreach (var kvp in updatedValues)
                    {
                        updateCommand.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                    }
                    updateCommand.ExecuteNonQuery();
                }
            }
        }
        // Delete data
        public void DeleteData(string tableName, Dictionary<string, object> conditions)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var whereClause = string.Join(" AND ", conditions.Keys.Select(k => $"{k} = @{k}"));
                string deleteQuery = $"DELETE FROM {tableName} WHERE {whereClause};";

                using (var deleteCommand = new SQLiteCommand(deleteQuery, connection))
                {
                    foreach (var kvp in conditions)
                    {
                        deleteCommand.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                    }
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }
        private IEnumerable<IDictionary<string, object>> GetPotatoData()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "TITLE", resources["str_recipe01"] },
                    { "VALUE", 100 },
                    { "ImageSource", resources["icon_potato"] },
                    { "Deleteable", 0 }
                },
            };
        }

    }
}