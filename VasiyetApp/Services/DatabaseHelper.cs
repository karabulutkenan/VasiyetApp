using System;
using System.Collections.Generic;
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

                // Kullanıcı tablosu mevcut değilse oluştur
                command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Surname TEXT,
                    TCKN TEXT,
                    Phone TEXT,
                    Username TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    ProfilePhotoPath TEXT
                );
                ";
                command.ExecuteNonQuery();

                // ProfilePhotoPath alanı eksikse tabloyu güncelle
                command.CommandText = "PRAGMA table_info(Users);";
                bool profilePhotoExists = false;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() == "ProfilePhotoPath")
                        {
                            profilePhotoExists = true;
                            break;
                        }
                    }
                }

                // ProfilePhotoPath sütunu mevcut değilse ekle
                if (!profilePhotoExists)
                {
                    command.CommandText = "ALTER TABLE Users ADD COLUMN ProfilePhotoPath TEXT;";
                    command.ExecuteNonQuery();
                }

                // Guardians ve Wills tablolarını oluştur
                command.CommandText =
                @"
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
    TextContent TEXT, 
    WordFilePath TEXT,
    MediaFilePath TEXT,
    DateAdded TEXT,
    UserId INTEGER,
    GuardianId INTEGER,
    FOREIGN KEY(UserId) REFERENCES Users(Id),
    FOREIGN KEY(GuardianId) REFERENCES Guardians(Id)
);
                ";
                command.ExecuteNonQuery();
            }
        }

        public static bool IsTableExists(string tableName)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
                var result = command.ExecuteScalar();
                return result != null;
            }
        }

        public static void AddUser(User user)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Users (Username, Name, Surname, TCKN, Phone, Email, Password, ProfilePhotoPath) VALUES (@Username, @Name, @Surname, @TCKN, @Phone, @Email, @Password, @ProfilePhotoPath)";
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Surname", user.Surname);
                command.Parameters.AddWithValue("@TCKN", user.TCKN);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@ProfilePhotoPath", user.ProfilePhotoPath);
                command.ExecuteNonQuery();
            }
        }

        public static User ValidateUser(string username, string password)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Username, Email, ProfilePhotoPath FROM Users WHERE Username = @Username AND Password = @Password";
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Username = reader.GetString(2),
                            Email = reader.GetString(3),
                            ProfilePhotoPath = reader.IsDBNull(4) ? null : reader.GetString(4)
                        };
                    }
                }
            }
            return null;
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

        public static void AddWill(Will will)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Wills (Title, Details, TextContent, WordFilePath, MediaFilePath, DateAdded, UserId, GuardianId) 
VALUES (@Title, @Details, @TextContent, @WordFilePath, @MediaFilePath, @DateAdded, @UserId, @GuardianId)";
                command.Parameters.AddWithValue("@Title", will.Title);
                command.Parameters.AddWithValue("@Details", will.Details);
                command.Parameters.AddWithValue("@TextContent", will.TextContent ?? (object)DBNull.Value); // Yeni alan
                command.Parameters.AddWithValue("@WordFilePath", will.WordFilePath ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MediaFilePath", will.MediaFilePath ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateAdded", will.DateAdded.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@UserId", will.UserId);

                if (will.GuardianId.HasValue)
                    command.Parameters.AddWithValue("@GuardianId", will.GuardianId.Value);
                else
                    command.Parameters.AddWithValue("@GuardianId", DBNull.Value);

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
                command.CommandText = @"SELECT Id, Title, Details, TextContent, WordFilePath, MediaFilePath, DateAdded, UserId, GuardianId 
FROM Wills WHERE UserId = @UserId";

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
                            TextContent = reader.IsDBNull(3) ? null : reader.GetString(3), // Yeni alan
                            WordFilePath = reader.IsDBNull(4) ? null : reader.GetString(4),
                            MediaFilePath = reader.IsDBNull(5) ? null : reader.GetString(5),
                            DateAdded = DateTime.Parse(reader.GetString(6)),
                            UserId = reader.GetInt32(7),
                            GuardianId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                            GuardianName = GetGuardianNameById(reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8))
                        });
                    }
                }
            }

            return wills;
        }


        public static void UpdateWill(Will will)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
        UPDATE Wills 
        SET 
            Title = @Title, 
            Details = @Details, 
            TextContent = @TextContent, 
            WordFilePath = @WordFilePath, 
            MediaFilePath = @MediaFilePath, 
            GuardianId = @GuardianId 
        WHERE Id = @Id";

                command.Parameters.AddWithValue("@Title", will.Title);
                command.Parameters.AddWithValue("@Details", will.Details);
                command.Parameters.AddWithValue("@TextContent", will.TextContent ?? (object)DBNull.Value); // Yeni alan
                command.Parameters.AddWithValue("@WordFilePath", will.WordFilePath ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@MediaFilePath", will.MediaFilePath ?? (object)DBNull.Value);

                if (will.GuardianId.HasValue)
                    command.Parameters.AddWithValue("@GuardianId", will.GuardianId.Value);
                else
                    command.Parameters.AddWithValue("@GuardianId", DBNull.Value);

                command.Parameters.AddWithValue("@Id", will.Id);
                command.ExecuteNonQuery();
            }
        }


        public static void DeleteWill(int willId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Wills WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", willId);
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteGuardian(int guardianId)
        {
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Guardians WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", guardianId);
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateGuardian(Guardian guardian)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE Guardians SET Name = @Name, Email = @Email WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Name", guardian.Name);
                    command.Parameters.AddWithValue("@Email", guardian.Email);
                    command.Parameters.AddWithValue("@Id", guardian.Id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"UpdateGuardian hata: {ex.Message}");
            }
        }
        public static List<Guardian> GetGuardians()
        {
            var guardians = new List<Guardian>();

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name FROM Guardians";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        guardians.Add(new Guardian
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
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

        public static string GetGuardianNameById(int? guardianId)
        {
            if (guardianId == null)
                return "Bilinmiyor";

            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Name FROM Guardians WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", guardianId);

                var result = command.ExecuteScalar();
                return result != null ? result.ToString() : "Bilinmiyor";
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
    }
}