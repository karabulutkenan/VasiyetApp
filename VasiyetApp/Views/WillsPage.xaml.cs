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
            var currentUser = App.CurrentUser;
            Wills = new ObservableCollection<Will>();
            WillsCollectionView.ItemsSource = Wills;
            ProfilePhotoImage.Source = string.IsNullOrEmpty(currentUser.ProfilePhotoPath)
                ? "default_profile.png"
                : currentUser.ProfilePhotoPath;
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

            // Bo� liste durumu kontrol�
            if (Wills.Count == 0)
            {
                WillsCollectionView.IsVisible = false;
                EmptyStateMessage.IsVisible = true;
            }
            else
            {
                WillsCollectionView.IsVisible = true;
                EmptyStateMessage.IsVisible = false;
            }
        }

        private async void OnViewGuardiansClicked(object sender, EventArgs e)
        {
            var guardiansPage = new GuardiansPage();
            await Navigation.PushModalAsync(guardiansPage);
        }

        private async void OnShowGuardiansClicked(object sender, EventArgs e)
        {
            try
            {
                // GuardiansPage'e y�nlendirme
                await Navigation.PushModalAsync(new GuardiansPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata olu�tu: {ex.Message}", "Tamam");
            }
        }


        private async void OnAddWillClicked(object sender, EventArgs e)
        {
            if (!DatabaseHelper.HasGuardians(App.CurrentUser.Id))
            {
                await DisplayAlert("Uyar�", "L�tfen �nce bir vasi ekleyiniz.", "Tamam");
                return;
            }

            var addWillPage = new AddWillPage();
            addWillPage.WillAdded += OnWillAdded;
            await Navigation.PushModalAsync(addWillPage);
        }

        private void OnWillAdded(object sender, Will newWill)
        {
            Wills.Add(newWill);
        }

        private async void OnAddGuardianClicked(object sender, EventArgs e)
        {
            var addGuardianPage = new AddGuardianPage();
            await Navigation.PushModalAsync(addGuardianPage);
        }

        private async void OnWillTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Will selectedWill)
            {
                var editWillPage = new EditWillPage(selectedWill);
                await Navigation.PushModalAsync(editWillPage);
            }
        }


        private async void OnExitIconTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnUserIconTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Uyar�", "Profil sayfas� hen�z haz�r de�il.", "Tamam");
        }
    }
}
