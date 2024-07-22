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
            // ��k�� i�lemi s�ras�nda t�m sayfa y���n�n� temizle ve LoginPage'e y�nlendir
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnCancel(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(".."); // Bir �nceki sayfaya d�n
        }
    }
}
