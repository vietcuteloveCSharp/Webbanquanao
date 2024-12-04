using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Cuahangs
{
    public class FullCuahangDTO
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
       
        public string Sdt { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
      
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
