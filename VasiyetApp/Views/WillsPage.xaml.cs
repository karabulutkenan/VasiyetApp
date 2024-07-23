using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using VasiyetApp.Models;

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
            var addWillPage = new AddWillPage();
            addWillPage.WillAdded += OnWillAdded;
            await Navigation.PushModalAsync(addWillPage);
        }

        private void OnWillAdded(object sender, Will newWill)
        {
            Wills.Add(newWill);
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
            // Will listesi güncellenebilir burada
        }
    }
}
