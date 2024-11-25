using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Sanphams
{
    public class SanPhamFilterDTO
    {
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public int SoLuong { get; set; } = 1;
        public string Gia { get; set; }
        public bool TrangThai { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? NgayCapNhat { get; set; }
        public string HinhAnh { get; set; } = string.Empty;
        public string TenThuongHieu { get; set; }
        public string TenDanhMuc { get; set; }
    }
}
