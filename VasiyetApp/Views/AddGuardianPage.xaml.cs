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
                await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurunuz.", "Tamam");
                return;
            }

            var guardian = new Guardian
            {
                Name = nameEntry.Text,
                Email = emailEntry.Text,
                UserId = App.CurrentUser.Id
            };

            DatabaseHelper.AddGuardian(guardian);

            // GuardianAdded olayýný tetikle
            GuardianAdded?.Invoke(this, guardian);

            await DisplayAlert("Baþarýlý", "Vasi baþarýyla kaydedildi.", "Tamam");
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
