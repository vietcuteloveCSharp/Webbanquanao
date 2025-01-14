using DAL.Entities;
using Enum.EnumVVA;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebView.NghiaDTO
{
    public class HoaDonDTO
    {
        public int Id { get; set; }
        public decimal TongTien { get; set; } = 0;// tổng tiền = sản phẩm + Phí vận chuyển - tiền từ mã giảm giá
        public decimal PhiVanChuyen { get; set; } = 0; // tiền phí vận chuyển
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public string? DiaChiGiaoHang { get; set; } = string.Empty;
        public ETrangThaiHD TrangThai { get; set; } = ETrangThaiHD.ChoXacNhan;

        [ForeignKey("NhanVien")]
        public int? Id_NhanVien { get; set; }
        [ForeignKey("KhachHang")]
        public int? Id_KhachHang { get; set; }

        public virtual NhanVien? NhanVien { get; set; }
        public virtual KhachHang? KhachHang { get; set; }

        public virtual ICollection<ThanhToanHoaDon> ThanhToanHoaDons { get; set; }
        public virtual ICollection<ChiTietHoaDonDTO> ChiTietHoaDons { get; set; }
    }
}
