using Microsoft.Maui.Controls;
using System;
using System.IO;
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
        }

        private void BindWillDetails()
        {
            // Mevcut bilgileri doldur
            TitleEntry.Text = _currentWill.Title;
            DetailsEditor.Text = _currentWill.Details;

            // Elle yaz�lm�� metin
            if (!string.IsNullOrEmpty(_currentWill.TextContent))
            {
                WrittenWillEditor.Text = _currentWill.TextContent;
                WrittenWillEditor.IsVisible = true;
                WrittenWillStatus.IsVisible = false;
            }
            else
            {
                WrittenWillEditor.Text = string.Empty;
                WrittenWillEditor.IsVisible = false;
                WrittenWillStatus.Text = "Elle yaz�lm�� metin yok.";
                WrittenWillStatus.IsVisible = true;
            }

            // Eklenen dosya
            if (!string.IsNullOrEmpty(_currentWill.WordFilePath))
            {
                AttachedFileLabel.Text = Path.GetFileName(_currentWill.WordFilePath);
            }
            else
            {
                AttachedFileLabel.Text = "Eklenen dosya yok.";
            }

            // Eklenen medya
            if (!string.IsNullOrEmpty(_currentWill.MediaFilePath))
            {
                AttachedMediaImage.Source = _currentWill.MediaFilePath;
                AttachedMediaImage.IsVisible = true;
                MediaStatus.IsVisible = false;
            }
            else
            {
                AttachedMediaImage.IsVisible = false;
                MediaStatus.Text = "Medya eklenmemi�.";
                MediaStatus.IsVisible = true;
            }

            // Vasi
            var guardians = DatabaseHelper.GetGuardians();
            GuardianPicker.ItemsSource = guardians;
            GuardianPicker.SelectedItem = guardians.FirstOrDefault(g => g.Id == _currentWill.GuardianId);

            // Ba�lang��ta d�zenleme kapal�
            SetEditMode(false);
        }

        private void SetEditMode(bool isEditable)
        {
            TitleEntry.IsEnabled = isEditable;
            DetailsEditor.IsEnabled = isEditable;
            WrittenWillEditor.IsEnabled = isEditable;
            GuardianPicker.IsEnabled = isEditable;
            OnSaveButton.IsEnabled = isEditable;
            ChangeFileButton.IsVisible = isEditable;
            ChangeMediaButton.IsVisible = isEditable;
        }

        private void OnEditClicked(object sender, EventArgs e)
        {
            SetEditMode(true);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                _currentWill.Title = TitleEntry.Text;
                _currentWill.Details = DetailsEditor.Text;

                // Elle yaz�lm�� metni kaydet
                if (!string.IsNullOrWhiteSpace(WrittenWillEditor.Text))
                {
                    _currentWill.TextContent = WrittenWillEditor.Text;
                }

                // Vasi se�imi
                if (GuardianPicker.SelectedItem is Guardian selectedGuardian)
                {
                    _currentWill.GuardianId = selectedGuardian.Id;
                }

                // Veritaban�n� g�ncelle
                DatabaseHelper.UpdateWill(_currentWill);

                await DisplayAlert("Ba�ar�l�", "Vasiyet ba�ar�yla g�ncellendi.", "Tamam");

                // D�zenleme modundan ��k
                SetEditMode(false);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata olu�tu: {ex.Message}", "Tamam");
            }
        }

        private async void OnChangeFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                _currentWill.WordFilePath = result.FullPath;
                AttachedFileLabel.Text = Path.GetFileName(result.FullPath);
            }
        }

        private async void OnChangeMediaClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Medya Se�in",
                FileTypes = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, new[] { "image/*", "video/*" } },
                        { DevicePlatform.iOS, new[] { "public.image", "public.movie" } }
                    })
            });

            if (result != null)
            {
                _currentWill.MediaFilePath = result.FullPath;
                AttachedMediaImage.Source = result.FullPath;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool isConfirmed = await DisplayAlert("Sil", "Bu vasiyeti silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");
            if (isConfirmed)
            {
                DatabaseHelper.DeleteWill(_currentWill.Id);
                await DisplayAlert("Silindi", "Vasiyet silindi.", "Tamam");
                await Navigation.PopModalAsync();
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
