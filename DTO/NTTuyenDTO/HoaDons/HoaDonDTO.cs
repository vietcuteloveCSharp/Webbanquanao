using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NTTuyen.HoaDons
{
    public class HoaDonDTO
    {

        public decimal TongTien { get; set; } // tổng tiền = sản phẩm + Phí vận chuyển - tiền từ mã giảm giá
        public decimal PhiVanChuyen { get; set; } // tiền phí vận chuyển
        public DateTime NgayTao { get; set; } 
        public int TrangThai { get; set; }
        [Required(ErrorMessage = "Id Nhân viên là bắt buộc")]
        public int Id_NhanVien { get; set; }
        [Required(ErrorMessage = "Id Khách hàng là bắt buộc")]
        public int Id_KhachHang { get; set; }

    }
}
