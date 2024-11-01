using Microsoft.Maui.Controls;
using System;
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
            LoadGuardians(); // Mevcut vasiler y�klenir
        }

        private void BindWillDetails()
        {
            // Veriyi bile�enlere ba�lama
            TitleEntry.Text = _currentWill.Title;
            DetailsEditor.Text = _currentWill.Details;
            GuardianPicker.SelectedItem = _currentWill.GuardianName;
            PhotoImage.Source = _currentWill.FilePath;

            // D�zenle moduna girmeden t�m alanlar� devre d��� b�rak�yoruz
            TitleEntry.IsEnabled = false;
            DetailsEditor.IsEnabled = false;
            GuardianPicker.IsEnabled = false;
            ChangePhotoButton.IsVisible = false;
        }

        private void LoadGuardians()
        {
            // Vasi listesini y�klemek i�in DatabaseHelper veya ilgili servis metotlar�n� �a��r�n
            var guardians = DatabaseHelper.GetGuardians();
            GuardianPicker.ItemsSource = guardians;
        }

        private void OnEditClicked(object sender, EventArgs e)
        {
            // D�zenle butonuna bas�ld���nda t�m alanlar� d�zenlenebilir hale getiriyoruz
            TitleEntry.IsEnabled = true;
            DetailsEditor.IsEnabled = true;
            GuardianPicker.IsEnabled = true;
            ChangePhotoButton.IsVisible = true;

            // "D�zenle" butonunu devre d���, "Kaydet" butonunu aktif hale getiriyoruz
            ((Button)sender).IsEnabled = false;
            OnSaveButton.IsEnabled = true;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // De�i�iklikleri kaydederken t�m alanlar� devre d��� b�rak�yoruz
            TitleEntry.IsEnabled = false;
            DetailsEditor.IsEnabled = false;
            GuardianPicker.IsEnabled = false;
            ChangePhotoButton.IsVisible = false;

            // Veritaban� kayd� i�in g�ncellenmi� verileri _currentWill nesnesine at�yoruz
            _currentWill.Title = TitleEntry.Text;
            _currentWill.Details = DetailsEditor.Text;
            _currentWill.GuardianName = GuardianPicker.SelectedItem?.ToString();

            // G�ncellenmi� vasiyeti veri taban�na kaydet
            DatabaseHelper.UpdateWill(_currentWill);

            await DisplayAlert("Ba�ar�l�", "Vasiyet ba�ar�yla g�ncellendi!", "Tamam");
            await Navigation.PopModalAsync(); // WillsPage'e geri d�n
        }

        private async void OnChangePhotoClicked(object sender, EventArgs e)
        {
            // Galeriden dosya se�me i�lemi
            var file = await FilePicker.PickAsync();
            if (file != null)
            {
                _currentWill.FilePath = file.FullPath;
                PhotoImage.Source = _currentWill.FilePath;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool isConfirmed = await DisplayAlert("Sil", "Bu vasiyeti silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");
            if (isConfirmed)
            {
                // Vasiyeti veri taban�ndan sil
                DatabaseHelper.DeleteWill(_currentWill.Id);

                await DisplayAlert("Silindi", "Vasiyet silindi.", "Tamam");
                await Navigation.PopModalAsync(); // WillsPage'e geri d�n
            }
        }
    }
}
