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
            LoadData();
        }

        private void LoadData()
        {
            var currentUser = App.CurrentUser;
            if (currentUser != null)
            {
                WelcomeLabel.Text = $"Ho�geldin, {currentUser.Name}!";
                ProfilePhotoImage.Source = currentUser.ProfilePhotoPath; // Profil foto�raf� yolu atan�yor
            }
            else
            {
                WelcomeLabel.Text = "Ho�geldin!";
            }

            var willsFromDb = DatabaseHelper.GetWillsByUserId(currentUser.Id);
            Wills.Clear();

            if (willsFromDb.Count == 0)
            {
                NoWillsMessage.IsVisible = true;
                WillsCarouselView.IsVisible = false;

                foreach (var will in willsFromDb)
                {
                    will.GuardianName = DatabaseHelper.GetGuardianNameById(will.GuardianId);
                    Wills.Add(will);
                }

                //WillsCountLabel.Text = $"Vasiyetlerim: {Wills.Count} Adet";
                WillsCountLabel.Text = $"Vasiyetlerim: {DatabaseHelper.GetWillsByUserId(currentUser.Id).Count} Adet";

                GuardiansCountLabel.Text = $"Vasilerim: {DatabaseHelper.GetGuardiansByUserId(currentUser.Id).Count} Ki�i";


            }
            else
            {
                NoWillsMessage.IsVisible = false;
                WillsCarouselView.IsVisible = true;

                foreach (var will in willsFromDb)
                {
                    will.GuardianName = DatabaseHelper.GetGuardianNameById(will.GuardianId);
                    Wills.Add(will);
                }

                //WillsCountLabel.Text = $"Vasiyetlerim: {Wills.Count} Adet";
                WillsCountLabel.Text = $"Vasiyetlerim: {DatabaseHelper.GetWillsByUserId(currentUser.Id).Count} Adet";
                
                GuardiansCountLabel.Text = $"Vasilerim: {DatabaseHelper.GetGuardiansByUserId(currentUser.Id).Count} Ki�i";
            }
        }

        // Vasiyet Ekle butonuna t�klan�nca WillsPage sayfas�na y�nlendirme
        private async void OnAddWillButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//WillsPage");
        }


        private async void OnUserIconTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Uyar�", "Profil sayfas� hen�z haz�r de�il.", "Tamam");
        }

        private async void OnExitIconTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
