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
                    { DevicePlatform.iOS, new[] { "public.image" } }, // iOS i�in �rnek
                    { DevicePlatform.Android, new[] { "image/*" } }, // Android i�in �rnek
                    { DevicePlatform.WinUI, new[] { ".jpg", ".png" } } // Windows i�in �rnek
                });

            var options = new PickOptions
            {
                PickerTitle = "Dosya Se�iniz",
                FileTypes = customFileType,
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                selectedFileName.Text = $"Se�ilen dosya: {result.FileName}";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Vasiyet kaydetme mant��� burada ger�ekle�ir
            await DisplayAlert("Ba�ar�l�", "Vasiyet ba�ar�yla kaydedildi.", "Tamam");
            // Kaydetme i�lemi tamamland�ktan sonra ana sayfaya d�n
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
