using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

using TyranoCupUwpApp.Shared.Models;

namespace TyranoCupUwpApp.Shared
{
    public class AccessDb
    {
        private readonly string _connectionString;

        public AccessDb()
        {
            _connectionString = $"Filename={Path.Combine(ApplicationData.Current.LocalFolder.Path, "AppData.db")}";
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("AppData.db", CreationCollisionOption.OpenIfExists);
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS SaveAudioModel (
                    AppointmentId TEXT PRIMARY KEY,
                    AudioId TEXT
                )
            ";

                command.ExecuteNonQuery();
            }
        }

        public void Create(SaveAudioModel model)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                INSERT INTO SaveAudioModel (AppointmentId, AudioId)
                VALUES ($AppointmentId, $AudioId)
            ";

                command.Parameters.AddWithValue("$AppointmentId", model.AppointmentId);
                command.Parameters.AddWithValue("$AudioId", model.AudioId);

                command.ExecuteNonQuery();
            }
        }

        public SaveAudioModel Read(string appointmentId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                SELECT AppointmentId, AudioId
                FROM SaveAudioModel
                WHERE AppointmentId = $AppointmentId
            ";

                command.Parameters.AddWithValue("$AppointmentId", appointmentId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new SaveAudioModel
                        {
                            AudioId = reader.GetString(0),
                            AppointmentId = reader.GetString(1)
                        };
                    }
                }
            }

            return null;
        }

        public void Update(SaveAudioModel model)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                UPDATE SaveAudioModel
                SET AudioId = $AudioId
                WHERE AppointmentId = $AppointmentId
            ";

                command.Parameters.AddWithValue("$AppointmentId", model.AppointmentId);
                command.Parameters.AddWithValue("$AudioId", model.AudioId);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(string audioId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                DELETE FROM SaveAudioModel
                WHERE AppointmentId = $AppointmentId
            ";

                command.Parameters.AddWithValue("$AppointmentId", audioId);

                command.ExecuteNonQuery();
            }
        }
    }
}
