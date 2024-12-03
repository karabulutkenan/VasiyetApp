﻿using Microsoft.Data.Sqlite;
using Microsoft.Maui.Storage;  // FileSystem için gerekli
using VasiyetApp.Models;  // User modelini kullanmak için

namespace VasiyetApp;

public partial class App : Application
{
    // Veritabanı dosyasının yolunu düzelt
    public static string DbPath = Path.Combine(FileSystem.AppDataDirectory, "vasiyet.db");

    // CurrentUser özelliği
    public static User CurrentUser { get; set; }

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
            Name TEXT,
            Surname TEXT,
            TCKN TEXT,
            Phone TEXT,
            Username TEXT NOT NULL,
            Password TEXT NOT NULL,
            Email TEXT NOT NULL,
            ProfilePhotoPath TEXT  -- Yeni profil fotoğrafı alanı eklendi
        );

        CREATE TABLE IF NOT EXISTS Wills (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            Title TEXT NOT NULL,
            Details TEXT NOT NULL,
            FilePath TEXT,
            GuardianId INTEGER,
            FOREIGN KEY (UserId) REFERENCES Users (Id),
            FOREIGN KEY (GuardianId) REFERENCES Guardians (Id)
        );

        CREATE TABLE IF NOT EXISTS Guardians (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT,
            Email TEXT,
            UserId INTEGER,
            FOREIGN KEY(UserId) REFERENCES Users(Id)
        );
        ";
            command.ExecuteNonQuery();

            // Eksik sütunlar için kontroller ve eklemeler
            AddColumnIfNotExists(db, "Wills", "TextFilePath", "TEXT");
            AddColumnIfNotExists(db, "Wills", "WordFilePath", "TEXT");
            AddColumnIfNotExists(db, "Wills", "MediaFilePath", "TEXT");
            AddColumnIfNotExists(db, "Wills", "DateAdded", "TEXT");
        }
    }

    // Eksik sütun eklemek için yardımcı metot
    private void AddColumnIfNotExists(SqliteConnection connection, string tableName, string columnName, string columnType)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info({tableName});";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                if (reader["name"].ToString() == columnName)
                {
                    return; // Sütun zaten var
                }
            }
        }

        // Sütun yoksa ekle
        var alterCommand = connection.CreateCommand();
        alterCommand.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnType};";
        alterCommand.ExecuteNonQuery();
    }

}