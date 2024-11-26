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
                // Vasi bilgilerini g�ncelle
                _currentGuardian.Name = NameEntry.Text;
                _currentGuardian.Email = EmailEntry.Text;

                DatabaseHelper.UpdateGuardian(_currentGuardian);

                await DisplayAlert("Ba�ar�l�", "Vasi ba�ar�yla g�ncellendi.", "Tamam");

                // GuardiansPage'e d�n
                await Shell.Current.GoToAsync("//WillsPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata olu�tu: {ex.Message}", "Tamam");
            }
        }


    }
}
