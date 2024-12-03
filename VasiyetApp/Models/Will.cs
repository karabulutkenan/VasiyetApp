using System;

namespace VasiyetApp.Models
{
    public class Will
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string TextFilePath { get; set; } // Elle yazılan vasiyet için
        public string WordFilePath { get; set; } // Word dosyası için
        public string MediaFilePath { get; set; } // Resim veya video dosyası için
        public string FilePath { get; set; } // Genel dosya yolu (isteğe bağlı)
        public DateTime DateAdded { get; set; } // Vasiyetin eklendiği tarih
        public int UserId { get; set; }
        public int? GuardianId { get; set; }
        public string GuardianName { get; set; }
    }
}
