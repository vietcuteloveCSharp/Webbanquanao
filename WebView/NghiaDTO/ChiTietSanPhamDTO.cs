using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO{
    public class ChiTietSanPhamDTO
    {
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool? TrangThai { get; set; }  // nullable để xử lý checkbox

        public int Id_SanPham { get; set; }
        public int Id_MauSac { get; set; }
        public int Id_KichThuoc { get; set; }
        public string TenMauSac { get; set; } = string.Empty;
        public string TenKichThuoc { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;  
        public int ID_DanhMuc { get; set; }
        public int ID_ThuongHieu { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        public string TenThuongHieu { get; set; } = string.Empty;
         public string HinhAnh { get; set; } = string.Empty;  // Thêm trường HinhAnh
                                                              // Thêm trường Gia
        public decimal Gia { get; set; }  
        public List<SanPhamDTO> sanPhamDTOs{ get; set; }
        public List<string> HinhAnhList { get; set; } = new List<string>();


    }

}
