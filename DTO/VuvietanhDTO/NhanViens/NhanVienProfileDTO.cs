using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.NhanViens
{
    public class NhanVienProfileDTO
    {
        public  int Id { get; set; }
        public string TaiKhoan { get; set; } = null!;
        public string Ten { get; set; } = string.Empty;
        public string Sdt { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgaySinh { get; set; } = DateTime.Now;
        public string DiaChi { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public int Id_ChucVu { get; set; }

    }
}
