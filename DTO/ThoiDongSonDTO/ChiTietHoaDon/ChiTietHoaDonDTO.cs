using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ThoiDongSonDTO.ChiTietHoaDon
{
    public class ChiTietHoaDonDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int SoLuong { get; set; }
        public string Gia { get; set; }
        public bool TrangThai { get; set; } = true;
        [Required(ErrorMessage = "Id Hóa đơn là bắt buộc.")]
        public int Id_HoaDon { get; set; }
        [Required(ErrorMessage = "Id Chi tiết sản phẩm là bắt buộc.")]
        public int Id_ChiTietSanPham { get; set; }
    }
}
