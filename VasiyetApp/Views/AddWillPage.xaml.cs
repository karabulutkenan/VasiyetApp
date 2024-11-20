using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using VasiyetApp.Models;
using VasiyetApp.Services;
using System.Collections.ObjectModel;

namespace VasiyetApp.Views
{
    public partial class AddWillPage : ContentPage
    {
        private string selectedFilePath;
        public event EventHandler<Will> WillAdded;

        public ObservableCollection<Guardian> Guardians { get; set; }

        public AddWillPage()
        {
            InitializeComponent();
            try
            {
                Guardians = new ObservableCollection<Guardian>(DatabaseHelper.GetGuardiansByUserId(App.CurrentUser.Id));
                guardianPicker.ItemsSource = Guardians;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddWillPage constructor: {ex.Message}");
                DisplayAlert("Error", "An error occurred while initializing the page.", "OK");
            }
        }

        private async void OnChooseFileClicked(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnChooseFileClicked: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while choosing the file.", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.CurrentUser == null)
                {
                    await DisplayAlert("Hata", "Oturum a�m�� kullan�c� bulunamad�.", "Tamam");
                    return;
                }

                if (guardianPicker.SelectedItem == null)
                {
                    await DisplayAlert("Hata", "L�tfen bir vasi se�iniz.", "Tamam");
                    return;
                }

                var selectedGuardian = (Guardian)guardianPicker.SelectedItem;

                var will = new Will
                {
                    Title = titleEntry.Text,
                    Details = detailsEditor.Text,
                    FilePath = selectedFilePath,
                    UserId = App.CurrentUser.Id,
                    GuardianId = selectedGuardian.Id // Se�ilen Guardian��n Id�si atan�yor
                };

                DatabaseHelper.AddWill(will);

                // WillAdded olay�n� tetikle
                WillAdded?.Invoke(this, will);

                await DisplayAlert("Ba�ar�l�", "Vasiyet ba�ar�yla kaydedildi.", "Tamam");
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnSaveClicked: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while saving the will.", "OK");
            }
        }


        private async void OnAddGuardianClicked(object sender, EventArgs e)
        {
            try
            {
                var addGuardianPage = new AddGuardianPage();
                addGuardianPage.GuardianAdded += OnGuardianAdded;
                await Navigation.PushModalAsync(addGuardianPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnAddGuardianClicked: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while adding a guardian.", "OK");
            }
        }

        private void OnGuardianAdded(object sender, Guardian newGuardian)
        {
            try
            {
                Guardians.Add(newGuardian);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnGuardianAdded: {ex.Message}");
            }
        }
    }
}
