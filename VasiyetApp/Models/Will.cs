using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasiyetApp.Models
{
    public class Will
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string FilePath { get; set; }
        public int UserId { get; set; }
        public int? GuardianId { get; set; }
        public string GuardianName { get; set; }
    }
}


