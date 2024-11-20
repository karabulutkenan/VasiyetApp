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
            // �ifrelerin e�le�ip e�le�medi�ini kontrol et
            if (passwordEntry.Text != confirmPasswordEntry.Text)
            {
                await DisplayAlert("Hata", "Girilen �ifreler uyu�muyor.", "Tamam");
                return;
            }

            // Profil foto�raf�n�n se�ilip se�ilmedi�ini kontrol et
            if (string.IsNullOrEmpty(profilePhotoPath))
            {
                await DisplayAlert("Hata", "L�tfen bir profil foto�raf� se�iniz.", "Tamam");
                return;
            }

            // Di�er bilgilerin eksik olup olmad���n� kontrol et
            if (string.IsNullOrWhiteSpace(nameEntry.Text) ||
                string.IsNullOrWhiteSpace(surnameEntry.Text) ||
                string.IsNullOrWhiteSpace(tcNoEntry.Text) ||
                string.IsNullOrWhiteSpace(phoneEntry.Text) ||
                string.IsNullOrWhiteSpace(emailEntry.Text) ||
                string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await DisplayAlert("Hata", "L�tfen bilgilerinizi eksiksiz doldurun.", "Tamam");
                return;
            }

            // Kullan�c� bilgilerini olu�tur
            var user = new User
            {
                Name = nameEntry.Text,
                Surname = surnameEntry.Text,
                TCKN = tcNoEntry.Text,
                Phone = phoneEntry.Text,
                Email = emailEntry.Text,
                Username = emailEntry.Text, // Kullan�c� ad� olarak e-posta kullan�l�yor
                Password = passwordEntry.Text,
                ProfilePhotoPath = profilePhotoPath
            };

            try
            {
                DatabaseHelper.AddUser(user);
                await DisplayAlert("Kay�t Ba�ar�l�", "Kullan�c� ba�ar�yla kaydedildi.", "Tamam");

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Kay�t Ba�ar�s�z", "Bir hata olu�tu: " + ex.Message, "Tamam");
            }
        }

        private async void OnChoosePhotoClicked(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Profil foto�raf� se�",
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
