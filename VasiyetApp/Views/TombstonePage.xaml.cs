using System;
using Microsoft.Maui.Controls;

namespace VasiyetApp.Views
{
    public partial class TombstonePage : ContentPage
    {
        public TombstonePage()
        {
            InitializeComponent();
            LoadPosts();
        }

        private void LoadPosts()
        {
            // Veritabanýndan gönderi listesini çek
            // Her bir gönderi için UI elemanlarý dinamik olarak oluþturulur
        }
    }
}
