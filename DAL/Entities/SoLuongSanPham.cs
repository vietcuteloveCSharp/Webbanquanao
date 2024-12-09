using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class SoLuongSanPham
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu không được bỏ trống số lượng")]
        public int SoLuong { get; set; }

        public int SanPhamId { get; set; }
        public DateTime NgayTao { get; set; }
        public SanPham SanPham{ get; set; } // Liên kết với bảng Products (nếu có)
    }
}
