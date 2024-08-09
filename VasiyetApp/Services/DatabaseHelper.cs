using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using VasiyetApp.Models;

namespace VasiyetApp.Services
{
    public class DatabaseHelper
    {
        static string dbPath = App.DbPath;

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT UNIQUE,
                        Email TEXT UNIQUE,
                        Password TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Guardians (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Email TEXT,
                        UserId INTEGER,
                        FOREIGN KEY(UserId) REFERENCES Users(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Wills (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT,
                        Details TEXT,
                        FilePath TEXT,
                        UserId INTEGER,
                        GuardianId INTEGER,
                        FOREIGN KEY(UserId) REFERENCES Users(Id),
                        FOREIGN KEY(GuardianId) REFERENCES Guardians(Id)
                    );
                ";
                command.ExecuteNonQuery();
            }
        }

        public static void AddUser(User user)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.ExecuteNonQuery();
            }
        }

        public static User ValidateUser(string username, string password)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Username, Email FROM Users WHERE Username = @Username AND Password = @Password";
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2)
                        };
                    }
                }
            }
            return null;
        }

        public static void AddWill(Will will)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Wills (Title, Details, FilePath, UserId, GuardianId) VALUES (@Title, @Details, @FilePath, @UserId, @GuardianId)";
                command.Parameters.AddWithValue("@Title", will.Title);
                command.Parameters.AddWithValue("@Details", will.Details);
                command.Parameters.AddWithValue("@FilePath", will.FilePath);
                command.Parameters.AddWithValue("@UserId", will.UserId);
                command.Parameters.AddWithValue("@GuardianId", will.GuardianId);
                command.ExecuteNonQuery();
            }
        }

        public static List<Will> GetWillsByUserId(int userId)
        {
            var wills = new List<Will>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Title, Details, FilePath, UserId, GuardianId FROM Wills WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        wills.Add(new Will
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Details = reader.GetString(2),
                            FilePath = reader.IsDBNull(3) ? null : reader.GetString(3),
                            UserId = reader.GetInt32(4),
                            GuardianId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5)
                        });
                    }
                }
            }

            return wills;
        }

        public static void AddGuardian(Guardian guardian)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Guardians (Name, Email, UserId) VALUES (@Name, @Email, @UserId)";
                command.Parameters.AddWithValue("@Name", guardian.Name);
                command.Parameters.AddWithValue("@Email", guardian.Email);
                command.Parameters.AddWithValue("@UserId", guardian.UserId);
                command.ExecuteNonQuery();
            }
        }

        public static List<Guardian> GetGuardiansByUserId(int userId)
        {
            var guardians = new List<Guardian>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Email, UserId FROM Guardians WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        guardians.Add(new Guardian
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            UserId = reader.GetInt32(3)
                        });
                    }
                }
            }

            return guardians;
        }

        public static bool HasGuardians(int userId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM Guardians WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);
                var result = command.ExecuteScalar();
                return (long)result > 0;
            }
        }
    }
}
