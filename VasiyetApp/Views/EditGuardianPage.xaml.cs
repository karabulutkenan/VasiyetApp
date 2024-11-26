using Microsoft.Maui.Controls;
using System;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class EditGuardianPage : ContentPage
    {
        private Guardian _currentGuardian;

        public EditGuardianPage(Guardian guardian)
        {
            InitializeComponent();
            _currentGuardian = guardian;
            NameEntry.Text = guardian.Name;
            EmailEntry.Text = guardian.Email;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Vasi bilgilerini güncelle
                _currentGuardian.Name = NameEntry.Text;
                _currentGuardian.Email = EmailEntry.Text;

                DatabaseHelper.UpdateGuardian(_currentGuardian);

                await DisplayAlert("Baþarýlý", "Vasi baþarýyla güncellendi.", "Tamam");

                // GuardiansPage'e dön
                await Shell.Current.GoToAsync("//WillsPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
            }
        }


    }
}
