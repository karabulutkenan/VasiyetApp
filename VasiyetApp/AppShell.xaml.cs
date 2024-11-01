﻿using Microsoft.Maui.Controls;
using System;
using VasiyetApp.Views;

namespace VasiyetApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            SetupRouting();
        }

        private void SetupRouting()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(WillsPage), typeof(WillsPage));
            Routing.RegisterRoute(nameof(TombstonePage), typeof(TombstonePage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }

        // Kullanıcı ikonuna tıklanınca çalışacak olay
        private async void OnUserIconTapped(object sender, EventArgs e)
        {
            await Shell.Current.DisplayAlert("Uyarı", "Profil sayfası henüz hazır değil.", "Tamam");
        }

        // Çıkış ikonuna tıklanınca çalışacak olay
        private async void OnExitIconTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
