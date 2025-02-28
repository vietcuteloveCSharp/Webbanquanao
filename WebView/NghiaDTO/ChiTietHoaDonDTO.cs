using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebView.NghiaDTO
{
    public class ChiTietHoaDonDTO
    {
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public bool TrangThai { get; set; } = true;

        [ForeignKey("HoaDon")]
        public int Id_HoaDon { get; set; }
        [ForeignKey("ChiTietSanPham")]
        public int Id_ChiTietSanPham { get; set; }

        public virtual HoaDonDTO HoaDon { get; set; }
        public virtual ChiTietSanPhamDTO ChiTietSanPham { get; set; }
    }
}
