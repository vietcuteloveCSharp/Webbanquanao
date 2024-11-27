using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Giohangs
{
    public class GioHangDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int SoLuong { get; set; }
        public bool TrangThai { get; set; } = true;
        [Required(ErrorMessage = "Khách hàng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id Khách hàng phải lớn hơn 0.")]
        public int Id_KhachHang { get; set; }
        [Required(ErrorMessage = "Chi tiết sản phẩm là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id Chi tiết sản phẩm phải lớn hơn 0.")]
        public int Id_ChiTietSanPham { get; set; }
    }
}
