using Microsoft.Maui.Controls;
using System;

namespace VasiyetApp.Views
{
    public partial class ExitConfirmationPage : ContentPage
    {
        public ExitConfirmationPage()
        {
            InitializeComponent();
        }

        private async void OnExitConfirmed(object sender, EventArgs e)
        {
            // Çýkýþ iþlemi sýrasýnda tüm sayfa yýðýnýný temizle ve LoginPage'e yönlendir
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnCancel(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(".."); // Bir önceki sayfaya dön
        }
    }
}
