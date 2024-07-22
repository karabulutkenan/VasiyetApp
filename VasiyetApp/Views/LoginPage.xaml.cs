using System;
using Microsoft.Maui.Controls;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Kullan�c� ad� ve �ifre alanlar�n�n bo� olup olmad���n� kontrol et
            if (string.IsNullOrWhiteSpace(usernameEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await DisplayAlert("Hata", "Kullan�c� ad� ve �ifre alanlar� doldurulmal�d�r.", "Tamam");
                return; // Kullan�c� ad� veya �ifre bo� ise i�lemi durdur
            }

            // Kullan�c� do�rulamas�
            bool isValid = DatabaseHelper.ValidateUser(usernameEntry.Text, passwordEntry.Text);
            if (isValid)
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await DisplayAlert("Giri� Ba�ar�s�z", "Kullan�c� ad� veya �ifre yanl��.", "Tamam");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Kay�t sayfas�n� modal olarak a�
            await Shell.Current.Navigation.PushModalAsync(new RegisterPage());
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            // �ifremi unuttum sayfas�na y�nlendirme veya i�levsellik burada ger�ekle�tirilecek
            await DisplayAlert("Bilgilendirme", "�ifre yenileme i�levi hen�z aktif de�il.", "Tamam");
        }
    }
}
