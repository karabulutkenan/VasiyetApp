using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using VasiyetApp.Models;
using VasiyetApp.Services;

namespace VasiyetApp.Views
{
    public partial class AddWillPage : ContentPage
    {
        private string selectedWordFilePath;
        private string selectedMediaFilePath;

        public ObservableCollection<Guardian> Guardians { get; set; }

        public AddWillPage()
        {
            InitializeComponent();
            Guardians = new ObservableCollection<Guardian>(DatabaseHelper.GetGuardiansByUserId(App.CurrentUser.Id));
            guardianPicker.ItemsSource = Guardians;
        }

        private void OnWriteWillClicked(object sender, EventArgs e)
        {
            // Yazý alanýný aç, butonu pasifleþtir
            WriteWillArea.IsVisible = true;
            OnWriteWillClickedButton.IsEnabled = false;
        }

        private void OnCancelWriteWillClicked(object sender, EventArgs e)
        {
            // Alaný gizle ve temizle
            WriteWillArea.IsVisible = false;
            WillTextEditor.Text = string.Empty;
            OnWriteWillClickedButton.IsEnabled = true;
        }

        private async void OnAttachWordFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                selectedWordFilePath = result.FullPath;
                selectedFileName.Text = $"Seçilen dosya: {result.FileName}";
            }
        }

        private async void OnAttachMediaClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Medya Seçin",
                FileTypes = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, new[] { "image/*", "video/*" } },
                        { DevicePlatform.iOS, new[] { "public.image", "public.movie" } }
                    })
            });

            if (result != null)
            {
                selectedMediaFilePath = result.FullPath;
                selectedFileName.Text = $"Seçilen dosya: {result.FileName}";
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Kullanýcýyý önceki sayfaya yönlendirme
            await Shell.Current.GoToAsync("..");
        }


        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.CurrentUser == null)
                {
                    await DisplayAlert("Hata", "Oturum açmýþ kullanýcý bulunamadý.", "Tamam");
                    return;
                }

                if (guardianPicker.SelectedItem == null)
                {
                    await DisplayAlert("Hata", "Lütfen bir vasi seçiniz.", "Tamam");
                    return;
                }

                // En az bir ekleme kontrolü
                if (string.IsNullOrWhiteSpace(WillTextEditor.Text) &&
                    string.IsNullOrEmpty(selectedWordFilePath) &&
                    string.IsNullOrEmpty(selectedMediaFilePath))
                {
                    await DisplayAlert("Hata", "Lütfen en az bir içerik ekleyin.", "Tamam");
                    return;
                }

                var selectedGuardian = (Guardian)guardianPicker.SelectedItem;

                // Yeni vasiyet oluþturma
                var will = new Will
                {
                    Title = titleEntry.Text,
                    Details = detailsEditor.Text,
                    TextContent = WriteWillArea.IsVisible ? WillTextEditor.Text : null,
                    WordFilePath = selectedWordFilePath,
                    MediaFilePath = selectedMediaFilePath,
                    UserId = App.CurrentUser.Id,
                    GuardianId = selectedGuardian.Id,
                    DateAdded = DateTime.Now
                };

                DatabaseHelper.AddWill(will);

                await DisplayAlert("Baþarýlý", "Vasiyet baþarýyla kaydedildi.", "Tamam");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
            }
        }
    }
}
