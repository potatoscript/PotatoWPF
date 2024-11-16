using PotatoWPF.Data;
using PotatoWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace PotatoWPF.ViewModels
{
    public class Item1ViewModel : BaseViewModel
    {
        private readonly DBHelper dbHelper;
        SQLiteConnection connection;
        //public List<PotatoModel> DataList { get; } = new List<PotatoModel>();

        public List<string> PotatoOptions { get; set; }

        private ObservableCollection<PotatoModel> _dataList;
        public ObservableCollection<PotatoModel> DataList
        {
            get => _dataList;
            set => SetProperty(ref _dataList, value);
        }
        public Item1ViewModel() 
        {
            dbHelper = new DBHelper();

            DataList = new ObservableCollection<PotatoModel>();

            PotatoOptions = new List<string>
            {
                "pack://application:,,,/Resources/Images/BakedPotato.jpg",
                "pack://application:,,,/Resources/Images/FondantPotatoes.jpg",
                "pack://application:,,,/Resources/Images/GarlicHerbRoastedPotatoes.jpg",
                "pack://application:,,,/Resources/Images/GreekLemonPotatoes.jpg",
                "pack://application:,,,/Resources/Images/LoadedBakedPotatoSoup.jpg",
                "pack://application:,,,/Resources/Images/TwiceBakedPotatoes.jpg"
            };
            /*
            var data = new PotatoModel
            {
                ID = 1,
                Title = "Potato",
                Type = "A",
                Value = 100,
                Deleteable = 1
            };
            DataList.Add(data);
            */

        }

        public void ApplyChanges(List<string> editedRowIds, List<string> deletedRowIds, List<PotatoModel> addedRowIds)
        {
            // Apply updates
            foreach (var id in editedRowIds)
            {
                var editedItem = DataList.FirstOrDefault(d => d.Id.ToString() == id);
                if (editedItem != null)
                {
                    UpdateData(editedItem);
                }
            }

            // Apply deletions
            foreach (var id in deletedRowIds)
            {
                DeleteData(id);
            }

            // Apply additions
            foreach (var newItem in addedRowIds)
            {
                InsertData(newItem);
            }
        }

        public void LoadAllData()
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={dbHelper.DbFilePath};Version=3;"))
                {
                    connection.Open();
                    string selectQuery = @"SELECT * FROM PotatoDBTable";
                    using (var command = new SQLiteCommand(selectQuery, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new PotatoModel
                                {
                                    Id = Convert.ToInt32(reader["ID"]),
                                    Title = reader["TITLE"].ToString(),
                                    Type = reader["TYPE"].ToString(),
                                    Value = reader.GetDouble(reader.GetOrdinal("VALUE")),
                                    ImageSource = reader["ImageSource"].ToString(),
                                    Deleteable = Convert.ToInt32(reader["Deleteable"])
                                };
                                DataList.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in LoadAllData: {ex.Message}");
            }
        }

        private void InsertData(PotatoModel model)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={dbHelper.DbFilePath};Version=3;"))
                {
                    connection.Open();
                    string insertQuery = @"
                INSERT INTO PotatoDBTable (TITLE, TYPE, VALUE,ImageSource, Deleteable)
                VALUES (@Title, @Type, @Value,@ImageSource, '1')";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", model.Title);
                        command.Parameters.AddWithValue("@Type", model.Type);
                        command.Parameters.AddWithValue("@Value", model.Value);
                        command.Parameters.AddWithValue("@ImageSource", model.ImageSource);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in InsertData: {ex.Message}");
            }
        }

        public void UpdateData(PotatoModel target)
        {
            using (connection = new SQLiteConnection($"Data Source={dbHelper.DbFilePath};Version=3;"))
            {
                connection.Open();
                string updateQuery = @"
                    UPDATE PotatoDBTable
                    SET TITLE = @Title, TYPE = @Type, VALUE = @Value, ImageSource = @ImageSource
                    WHERE ID = @ID";
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", target.Id);
                    command.Parameters.AddWithValue("@Title", target.Title);
                    command.Parameters.AddWithValue("@Type", target.Type);
                    command.Parameters.AddWithValue("@Value", target.Value);
                    command.Parameters.AddWithValue("@ImageSource", target.ImageSource);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteData(string id)
        {
            using (connection = new SQLiteConnection($"Data Source={dbHelper.DbFilePath};Version=3;"))
            {
                connection.Open();
                string deleteQuery = @"DELETE FROM PotatoDBTable WHERE ID = @ID";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
