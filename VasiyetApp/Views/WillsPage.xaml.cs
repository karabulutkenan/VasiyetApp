using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class WillsPage : ContentPage
    {
        public ObservableCollection<Will> Wills { get; set; }

        public WillsPage()
        {
            InitializeComponent();
            Wills = new ObservableCollection<Will>();
            WillsCollectionView.ItemsSource = Wills;
        }

        private async void OnAddWillClicked(object sender, EventArgs e)
        {
            if (!DatabaseHelper.HasGuardians(App.CurrentUser.Id))
            {
                await DisplayAlert("Uyarý", "Lütfen önce bir vasi ekleyiniz.", "Tamam");
                return;
            }

            var addWillPage = new AddWillPage();
            addWillPage.WillAdded += OnWillAdded;
            await Navigation.PushModalAsync(addWillPage);
        }

        private async void OnAddGuardianClicked(object sender, EventArgs e)
        {
            var addGuardianPage = new AddGuardianPage();
            addGuardianPage.GuardianAdded += OnGuardianAdded;
            await Navigation.PushModalAsync(addGuardianPage);
        }

        private void OnWillAdded(object sender, Will newWill)
        {
            Wills.Add(newWill);
        }

        private void OnGuardianAdded(object sender, Guardian newGuardian)
        {
            // Burada gerekli iþlemler yapýlabilir, örneðin vasilerin listesi güncellenebilir.
        }

        private async void OnExitIconTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnUserIconTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Uyarý", "Profil sayfasý henüz hazýr deðil.", "Tamam");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadWills();
        }

        private void LoadWills()
        {
            Wills.Clear();
            var willsFromDb = DatabaseHelper.GetWillsByUserId(App.CurrentUser.Id);
            foreach (var will in willsFromDb)
            {
                Wills.Add(will);
            }
        }
    }
}
