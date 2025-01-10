using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NTTuyen.HoaDons
{
    public class FullHoaDonDTO
    {
        public int Id { get; set; }
        public decimal TongTien { get; set; } // tổng tiền = sản phẩm + Phí vận chuyển - tiền từ mã giảm giá
        public decimal PhiVanChuyen { get; set; } // tiền phí vận chuyển
        public string? DiaChiGiaoHang { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
        public int TrangThai { get; set; }
        public int Id_NhanVien { get; set; }
        public int Id_KhachHang { get; set; }

    }
}
