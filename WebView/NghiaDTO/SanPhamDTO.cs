using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO
{
    public class SanPhamDTO
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public int SoLuong { get; set; } = 1;
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public bool TrangThai { get; set; }
        public string HinhAnh { get; set; } = string.Empty;
        public int Id_ThuongHieu { get; set; }
        public int Id_DanhMuc { get; set; }
        public int Id_KichThuoc { get; set; }
        public int Id_MauSac { get; set; }

        public string ThuongHieuTen { get; set; } = string.Empty;
        public string DanhMucTen { get; set; } = string.Empty;

        public List<ChiTietSanPhamDTO> ChiTietSanPhams { get; set; }
    }
}
