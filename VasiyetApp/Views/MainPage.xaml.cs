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
                WelcomeLabel.Text = $"Hoþgeldin, {currentUser.Name}!";
                ProfilePhotoImage.Source = currentUser.ProfilePhotoPath; // Profil fotoðrafý yolu atanýyor
            }
            else
            {
                WelcomeLabel.Text = "Hoþgeldin!";
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

                GuardiansCountLabel.Text = $"Vasilerim: {DatabaseHelper.GetGuardiansByUserId(currentUser.Id).Count} Kiþi";


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
                
                GuardiansCountLabel.Text = $"Vasilerim: {DatabaseHelper.GetGuardiansByUserId(currentUser.Id).Count} Kiþi";
            }
        }

        // Vasiyet Ekle butonuna týklanýnca WillsPage sayfasýna yönlendirme
        private async void OnAddWillButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//WillsPage");
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
