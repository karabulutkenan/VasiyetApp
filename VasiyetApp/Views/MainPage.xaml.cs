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
            // Profil sayfasý henüz hazýr olmadýðý için uyarý mesajý göster
            await DisplayAlert("Uyarý", "Profil sayfasý henüz hazýr deðil.", "Tamam");
        }

        private async void OnExitIconTapped(object sender, EventArgs e)
        {
            // Çýkýþ iþlemini gerçekleþtir
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
