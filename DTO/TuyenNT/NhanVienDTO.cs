using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.TuyenNT
{
    public class NhanVienDTO
    {
        
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string TenNhanVien { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public DateTime? NgaySinh { get; set; } 
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; } 
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int Id_ChucVu { get; set; }
    }
}
