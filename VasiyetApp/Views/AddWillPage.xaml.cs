using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace VasiyetApp.Views
{
    public partial class AddWillPage : ContentPage
    {
        public AddWillPage()
        {
            InitializeComponent();
        }

        private async void OnChooseFileClicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.image" } }, // iOS için örnek
                    { DevicePlatform.Android, new[] { "image/*" } }, // Android için örnek
                    { DevicePlatform.WinUI, new[] { ".jpg", ".png" } } // Windows için örnek
                });

            var options = new PickOptions
            {
                PickerTitle = "Dosya Seçiniz",
                FileTypes = customFileType,
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                selectedFileName.Text = $"Seçilen dosya: {result.FileName}";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Vasiyet kaydetme mantýðý burada gerçekleþir
            await DisplayAlert("Baþarýlý", "Vasiyet baþarýyla kaydedildi.", "Tamam");
            // Kaydetme iþlemi tamamlandýktan sonra ana sayfaya dön
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
