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

        public decimal TongTien { get; set; } 
        public decimal PhiVanChuyen { get; set; } 
        public DateTime NgayTao { get; set; }
        public string? DiaChiGiaoHang { get; set; } = string.Empty;
        public int TrangThai { get; set; }
        [Required(ErrorMessage = "Id Nhân viên là bắt buộc")]
        public int Id_NhanVien { get; set; } = 1;
        [Required(ErrorMessage = "Id Khách hàng là bắt buộc")]
        public int Id_KhachHang { get; set; }

    }
}
