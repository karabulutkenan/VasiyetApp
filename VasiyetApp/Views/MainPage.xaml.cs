using Microsoft.Maui.Controls;
using System;

namespace VasiyetApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnUserIconTapped(object sender, EventArgs e)
        {
            // Profil sayfas� hen�z haz�r olmad��� i�in uyar� mesaj� g�ster
            await DisplayAlert("Uyar�", "Profil sayfas� hen�z haz�r de�il.", "Tamam");
        }

        private async void OnExitIconTapped(object sender, EventArgs e)
        {
            // ��k�� i�lemini ger�ekle�tir
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
