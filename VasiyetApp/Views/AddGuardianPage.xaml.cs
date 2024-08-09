using Microsoft.Maui.Controls;
using System;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class AddGuardianPage : ContentPage
    {
        public event EventHandler<Guardian> GuardianAdded;

        public AddGuardianPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text) || string.IsNullOrWhiteSpace(emailEntry.Text))
            {
                await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurunuz.", "Tamam");
                return;
            }

            var guardian = new Guardian
            {
                Name = nameEntry.Text,
                Email = emailEntry.Text,
                UserId = App.CurrentUser.Id
            };

            DatabaseHelper.AddGuardian(guardian);

            // GuardianAdded olay�n� tetikle
            GuardianAdded?.Invoke(this, guardian);

            await DisplayAlert("Ba�ar�l�", "Vasi ba�ar�yla kaydedildi.", "Tamam");
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
