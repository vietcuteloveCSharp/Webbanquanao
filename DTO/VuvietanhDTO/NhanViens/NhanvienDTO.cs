using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.NhanViens
{
    public class NhanvienDTO
    {
        
        public string TaiKhoan { get; set; } = null!;
        public string TenNhanVien { get; set; } = string.Empty;
        public int Id_ChucVu { get; set; }
    }
}
