using Microsoft.Data.Sqlite;
using Microsoft.Maui.Storage;  // FileSystem için gerekli

namespace VasiyetApp;

public partial class App : Application
{
    // Veritabanı dosyasının yolunu düzelt
    public static string DbPath = Path.Combine(FileSystem.AppDataDirectory, "vasiyet.db");

    public App()
    {
        InitializeComponent();

        InitializeDatabase();

        MainPage = new AppShell();
    }

    private void InitializeDatabase()
    {
        // Veritabanı dosyasının olup olmadığını kontrol et
        if (!File.Exists(DbPath))
        {
            // Dosya yoksa, oluştur
            File.Create(DbPath).Dispose();
        }

        using (var db = new SqliteConnection($"Data Source={DbPath}"))
        {
            db.Open();

            var command = db.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    Email TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Wills (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Title TEXT NOT NULL,
                    Details TEXT NOT NULL,
                    GuardianId INTEGER,
                    FOREIGN KEY (UserId) REFERENCES Users (Id),
                    FOREIGN KEY (GuardianId) REFERENCES Users (Id)
                );
            ";
            command.ExecuteNonQuery();
        }
    }
}
