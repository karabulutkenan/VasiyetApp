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
            // Kullanýcý adý ve þifre alanlarýnýn boþ olup olmadýðýný kontrol et
            if (string.IsNullOrWhiteSpace(usernameEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await DisplayAlert("Hata", "Kullanýcý adý ve þifre alanlarý doldurulmalýdýr.", "Tamam");
                return; // Kullanýcý adý veya þifre boþ ise iþlemi durdur
            }

            // Kullanýcý doðrulamasý
            bool isValid = DatabaseHelper.ValidateUser(usernameEntry.Text, passwordEntry.Text);
            if (isValid)
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await DisplayAlert("Giriþ Baþarýsýz", "Kullanýcý adý veya þifre yanlýþ.", "Tamam");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Kayýt sayfasýný modal olarak aç
            await Shell.Current.Navigation.PushModalAsync(new RegisterPage());
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            // Þifremi unuttum sayfasýna yönlendirme veya iþlevsellik burada gerçekleþtirilecek
            await DisplayAlert("Bilgilendirme", "Þifre yenileme iþlevi henüz aktif deðil.", "Tamam");
        }
    }
}
