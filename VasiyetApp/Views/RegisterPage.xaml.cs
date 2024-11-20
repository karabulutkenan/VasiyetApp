using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        private string profilePhotoPath;

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Þifrelerin eþleþip eþleþmediðini kontrol et
            if (passwordEntry.Text != confirmPasswordEntry.Text)
            {
                await DisplayAlert("Hata", "Girilen þifreler uyuþmuyor.", "Tamam");
                return;
            }

            // Profil fotoðrafýnýn seçilip seçilmediðini kontrol et
            if (string.IsNullOrEmpty(profilePhotoPath))
            {
                await DisplayAlert("Hata", "Lütfen bir profil fotoðrafý seçiniz.", "Tamam");
                return;
            }

            // Diðer bilgilerin eksik olup olmadýðýný kontrol et
            if (string.IsNullOrWhiteSpace(nameEntry.Text) ||
                string.IsNullOrWhiteSpace(surnameEntry.Text) ||
                string.IsNullOrWhiteSpace(tcNoEntry.Text) ||
                string.IsNullOrWhiteSpace(phoneEntry.Text) ||
                string.IsNullOrWhiteSpace(emailEntry.Text) ||
                string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await DisplayAlert("Hata", "Lütfen bilgilerinizi eksiksiz doldurun.", "Tamam");
                return;
            }

            // Kullanýcý bilgilerini oluþtur
            var user = new User
            {
                Name = nameEntry.Text,
                Surname = surnameEntry.Text,
                TCKN = tcNoEntry.Text,
                Phone = phoneEntry.Text,
                Email = emailEntry.Text,
                Username = emailEntry.Text, // Kullanýcý adý olarak e-posta kullanýlýyor
                Password = passwordEntry.Text,
                ProfilePhotoPath = profilePhotoPath
            };

            try
            {
                DatabaseHelper.AddUser(user);
                await DisplayAlert("Kayýt Baþarýlý", "Kullanýcý baþarýyla kaydedildi.", "Tamam");

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Kayýt Baþarýsýz", "Bir hata oluþtu: " + ex.Message, "Tamam");
            }
        }

        private async void OnChoosePhotoClicked(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Profil fotoðrafý seç",
                FileTypes = FilePickerFileType.Images
            });

            if (file != null)
            {
                profilePhotoPath = file.FullPath;
                profilePhotoImage.Source = profilePhotoPath;
            }
        }
    }
}
