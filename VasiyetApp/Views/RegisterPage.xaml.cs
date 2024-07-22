using System;
using Microsoft.Maui.Controls;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (passwordEntry.Text != confirmPasswordEntry.Text)
            {
                await DisplayAlert("Hata", "Girilen �ifreler uyu�muyor.", "Tamam");
                return; // �ifreler uyu�muyorsa i�lemi durdur
            }

            var user = new User
            {
                Name = nameEntry.Text,
                Surname = surnameEntry.Text,
                TCKN = tcNoEntry.Text,
                Phone = phoneEntry.Text,
                Email = emailEntry.Text,
                Username = emailEntry.Text, // Kullan�c� ad� olarak e-posta adresi kullan�labilir
                Password = passwordEntry.Text  // Ger�ek uygulamada burada hashleme yap�lmal�
            };

            try
            {
                DatabaseHelper.AddUser(user);
                await DisplayAlert("Kay�t Ba�ar�l�", "Kullan�c� ba�ar�yla kaydedildi.", "OK");

                // Kay�t sonras� modal� kapat ve giri� sayfas�na geri d�n
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Kay�t Ba�ar�s�z", "Bir hata olu�tu: " + ex.Message, "Tamam");
            }
        }
    }
}
