using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Will> Wills { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Wills = new ObservableCollection<Will>();
            WillsCarouselView.ItemsSource = Wills;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData(); // Sayfa her göründüðünde verileri yenile
        }

        private void LoadData()
        {
            var currentUser = App.CurrentUser;
            if (currentUser != null)
            {
                WelcomeLabel.Text = $"Hoþgeldin, {currentUser.Name}!"; // Name özelliðini kullanýyoruz
            }
            else
            {
                WelcomeLabel.Text = "Hoþgeldin!";
            }

            var willsFromDb = DatabaseHelper.GetWillsByUserId(currentUser.Id);
            Wills.Clear();
            foreach (var will in willsFromDb)
            {
                will.GuardianName = DatabaseHelper.GetGuardianNameById(will.GuardianId);
                Wills.Add(will);
            }

            if (Wills.Count == 0)
            {
                Wills.Add(new Will { Title = "Vasiyet eklenmedi.", Details = "", GuardianName = "" });
            }

            WillsCountLabel.Text = $"Vasiyetlerim: {Wills.Count} Adet";
            GuardiansCountLabel.Text = $"Vasilerim: {DatabaseHelper.GetGuardiansByUserId(currentUser.Id).Count} Kiþi";
        }

        private async void OnUserIconTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Uyarý", "Profil sayfasý henüz hazýr deðil.", "Tamam");
        }

        private async void OnExitIconTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
