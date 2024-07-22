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
                await DisplayAlert("Hata", "Girilen þifreler uyuþmuyor.", "Tamam");
                return; // Þifreler uyuþmuyorsa iþlemi durdur
            }

            var user = new User
            {
                Name = nameEntry.Text,
                Surname = surnameEntry.Text,
                TCKN = tcNoEntry.Text,
                Phone = phoneEntry.Text,
                Email = emailEntry.Text,
                Username = emailEntry.Text, // Kullanýcý adý olarak e-posta adresi kullanýlabilir
                Password = passwordEntry.Text  // Gerçek uygulamada burada hashleme yapýlmalý
            };

            try
            {
                DatabaseHelper.AddUser(user);
                await DisplayAlert("Kayýt Baþarýlý", "Kullanýcý baþarýyla kaydedildi.", "OK");

                // Kayýt sonrasý modalý kapat ve giriþ sayfasýna geri dön
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Kayýt Baþarýsýz", "Bir hata oluþtu: " + ex.Message, "Tamam");
            }
        }
    }
}
