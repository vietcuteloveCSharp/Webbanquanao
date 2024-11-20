using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.TuyenNT
{
    public class ThanhToanHoaDonDTO
    {
     
        public string TongTien { get; set; }
        public string SoTienDaThanhToan { get; set; }
        public DateTime NgayThanhToan { get; set; } 
        public string MaGiaoDich { get; set; } 
        public int Id_HoaDon { get; set; }
        public int Id_PhuongThucThanhToan { get; set; }

    }
}
