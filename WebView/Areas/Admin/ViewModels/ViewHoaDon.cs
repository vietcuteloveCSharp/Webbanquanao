using Enum.EnumVVA;
using System.ComponentModel.DataAnnotations;

namespace WebView.Areas.Admin.ViewModels
{
    public class ViewHoaDon
    {
        public class KhachHangView
        {
            public int Id { get; set; }
            public string Ten { get; set; }
            public string Sdt { get; set; }
        }

        public class SanPhamView
        {
            public int Id { get; set; }
            public string Ten { get; set; }
            public int SoLuong { get; set; }
        }
        public class HoaDonView
        {

            public int Id { get; set; }
            public decimal TongTien { get; set; }
            public decimal PhiVanChuyen { get; set; }
            public DateTime NgayTao { get; set; }
            public int TrangThai { get; set; }
            public KhachHangView KhachHangs { get; set; } // Danh sách khách hàng
            public List<SanPhamView> SanPhams { get; set; } // Danh sách sản phẩm
        }
    }

}
