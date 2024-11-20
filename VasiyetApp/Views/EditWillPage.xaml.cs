using Microsoft.Maui.Controls;
using System;
using System.Linq;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class EditWillPage : ContentPage
    {
        private Will _currentWill;

        public EditWillPage(Will selectedWill)
        {
            InitializeComponent();
            _currentWill = selectedWill;
            BindWillDetails();
            LoadGuardians(); // Mevcut vasiler yüklenir
        }

        private void BindWillDetails()
        {
            // Veriyi bileþenlere baðlama
            TitleEntry.Text = _currentWill.Title;
            DetailsEditor.Text = _currentWill.Details;
            PhotoImage.Source = _currentWill.FilePath;

            // Düzenle moduna girmeden tüm alanlarý devre dýþý býrakýyoruz
            TitleEntry.IsEnabled = false;
            DetailsEditor.IsEnabled = false;
            GuardianPicker.IsEnabled = false;
            ChangePhotoButton.IsVisible = false;
        }

        private void LoadGuardians()
        {
            // Vasi listesini yüklemek için DatabaseHelper veya ilgili servis metotlarýný çaðýrýn
            var guardians = DatabaseHelper.GetGuardians();
            GuardianPicker.ItemsSource = guardians;

            // Mevcut vasiyi GuardianPicker'da seçili olarak göster
            var selectedGuardian = guardians.FirstOrDefault(g => g.Id == _currentWill.GuardianId);
            GuardianPicker.SelectedItem = selectedGuardian;
        }

        private void OnEditClicked(object sender, EventArgs e)
        {
            // Düzenle butonuna basýldýðýnda tüm alanlarý düzenlenebilir hale getiriyoruz
            TitleEntry.IsEnabled = true;
            DetailsEditor.IsEnabled = true;
            GuardianPicker.IsEnabled = true;
            ChangePhotoButton.IsVisible = true;

            // "Düzenle" butonunu devre dýþý, "Kaydet" butonunu aktif hale getiriyoruz
            ((Button)sender).IsEnabled = false;
            OnSaveButton.IsEnabled = true;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Deðiþiklikleri kaydederken tüm alanlarý devre dýþý býrakýyoruz
            TitleEntry.IsEnabled = false;
            DetailsEditor.IsEnabled = false;
            GuardianPicker.IsEnabled = false;
            ChangePhotoButton.IsVisible = false;

            // Veritabaný kaydý için güncellenmiþ verileri _currentWill nesnesine atýyoruz
            _currentWill.Title = TitleEntry.Text;
            _currentWill.Details = DetailsEditor.Text;

            // Guardian seçiliyse Id deðerini alýyoruz
            if (GuardianPicker.SelectedItem is Guardian selectedGuardian)
            {
                _currentWill.GuardianId = selectedGuardian.Id;
            }
            else
            {
                _currentWill.GuardianId = null;
            }

            // Güncellenmiþ vasiyeti veri tabanýna kaydet
            DatabaseHelper.UpdateWill(_currentWill);

            await DisplayAlert("Baþarýlý", "Vasiyet baþarýyla güncellendi!", "Tamam");
            await Navigation.PopModalAsync(); // WillsPage'e geri dön
        }


        private async void OnChangePhotoClicked(object sender, EventArgs e)
        {
            // Galeriden dosya seçme iþlemi
            var file = await FilePicker.PickAsync();
            if (file != null)
            {
                _currentWill.FilePath = file.FullPath;
                PhotoImage.Source = _currentWill.FilePath;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool isConfirmed = await DisplayAlert("Sil", "Bu vasiyeti silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (isConfirmed)
            {
                // Vasiyeti veri tabanýndan sil
                DatabaseHelper.DeleteWill(_currentWill.Id);

                await DisplayAlert("Silindi", "Vasiyet silindi.", "Tamam");
                await Navigation.PopModalAsync(); // WillsPage'e geri dön
            }
        }
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Herhangi bir deðiþiklik yapmadan WillsPage'e geri dön
            await Navigation.PopModalAsync();
        }

    }
}
