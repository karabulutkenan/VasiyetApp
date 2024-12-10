using System;

namespace VasiyetApp.Models
{
    public class Will
    {
        public int Id { get; set; }
        public string Title { get; set; } // Başlık
        public string Details { get; set; } // Detay
        public string TextContent { get; set; } // Elle yazılmış metin
        public string WordFilePath { get; set; } // Word dosyası
        public string MediaFilePath { get; set; } // Resim/video dosyası
        public DateTime DateAdded { get; set; } // Vasiyetin eklendiği tarih
        public int UserId { get; set; } // Kullanıcı
        public int? GuardianId { get; set; } // Vasi
        public string GuardianName { get; set; } // Vasi ismi
    }

}
