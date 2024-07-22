using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasiyetApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } // Bu alana e-posta atanacak
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TCKN { get; set; } // Türkiye Cumhuriyeti Kimlik Numarası
        public string Phone { get; set; }
    }

}
