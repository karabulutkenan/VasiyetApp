using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class GuardiansPage : ContentPage
    {
        public ObservableCollection<Guardian> Guardians { get; set; }

        public GuardiansPage()
        {
            InitializeComponent();
            Guardians = new ObservableCollection<Guardian>();
            GuardiansCollectionView.ItemsSource = Guardians;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadGuardians();
        }

        private void LoadGuardians()
        {
            Guardians.Clear();
            var guardiansFromDb = DatabaseHelper.GetGuardiansByUserId(App.CurrentUser.Id);
            foreach (var guardian in guardiansFromDb)
            {
                Guardians.Add(guardian);
            }
        }

        private async void OnEditGuardianClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Guardian selectedGuardian)
            {
                var editGuardianPage = new EditGuardianPage(selectedGuardian);
                await Navigation.PushModalAsync(editGuardianPage);
            }
        }

        private async void OnDeleteGuardianClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Guardian selectedGuardian)
            {
                bool confirm = await DisplayAlert("Sil", "Bu vasiyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
                if (confirm)
                {
                    DatabaseHelper.DeleteGuardian(selectedGuardian.Id);
                    Guardians.Remove(selectedGuardian);
                    await DisplayAlert("Baþarýlý", "Vasi silindi.", "Tamam");
                }
            }
        }
    }
}
