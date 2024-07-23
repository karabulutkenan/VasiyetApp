using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class AddWillPage : ContentPage
    {
        private string selectedFilePath;
        public event EventHandler<Will> WillAdded;

        public AddWillPage()
        {
            InitializeComponent();
        }

        private async void OnChooseFileClicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.image" } },
                    { DevicePlatform.Android, new[] { "image/*" } },
                    { DevicePlatform.WinUI, new[] { ".jpg", ".png" } }
                });

            var options = new PickOptions
            {
                PickerTitle = "Dosya Se�iniz",
                FileTypes = customFileType,
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                selectedFilePath = result.FullPath;
                selectedFileName.Text = $"Se�ilen dosya: {result.FileName}";
            }
            else
            {
                selectedFilePath = null;
                selectedFileName.Text = "Se�ilen dosya: Yok";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // App.CurrentUser ve selectedFilePath null kontrol�
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Hata", "Oturum a�m�� kullan�c� bulunamad�.", "Tamam");
                return;
            }

            if (string.IsNullOrEmpty(selectedFilePath))
            {
                await DisplayAlert("Hata", "L�tfen bir dosya se�iniz.", "Tamam");
                return;
            }

            var will = new Will
            {
                Title = titleEntry.Text,
                Details = detailsEditor.Text,
                FilePath = selectedFilePath,
                UserId = App.CurrentUser.Id
            };

            DatabaseHelper.AddWill(will);

            // WillAdded olay�n� tetikle
            WillAdded?.Invoke(this, will);

            await DisplayAlert("Ba�ar�l�", "Vasiyet ba�ar�yla kaydedildi.", "Tamam");
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
