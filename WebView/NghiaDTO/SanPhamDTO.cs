using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO
{
    public class SanPhamDTO
    {
        [Required(ErrorMessage = "Id là bắt buộc")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        public string Ten { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string MoTa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Gia { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 1")]
        public int SoLuong { get; set; } = 1;
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public bool TrangThai { get; set; } = true;
        public string HinhAnh { get; set; } = string.Empty;
        public int Id_ThuongHieu { get; set; }
        public int Id_DanhMuc { get; set; }
        public int Id_KichThuoc { get; set; }
        public int Id_MauSac { get; set; }

        public string ThuongHieuTen { get; set; } = string.Empty;
        public string DanhMucTen { get; set; } = string.Empty;
        public List<string> HinhAnhList { get; set; } = new List<string>();
        public List<ChiTietSanPhamDTO> ChiTietSanPhams { get; set; }
        public List<HinhAnhDTO> HinhAnhs { get; set; } = new();

    }
}
