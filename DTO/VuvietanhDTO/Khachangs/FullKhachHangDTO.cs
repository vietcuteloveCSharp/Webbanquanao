using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Khachangs
{
    public class FullKhachHangDTO
    {
        public int Id { get; set; }
        public string TaiKhoan { get; set; } 
        public string MatKhau { get; set; } 
        public DateTime? NgaySinh { get; set; }
        public string Ten { get; set; } 
        public string Sdt { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; } 
        public bool TrangThai { get; set; } 
    }
}
