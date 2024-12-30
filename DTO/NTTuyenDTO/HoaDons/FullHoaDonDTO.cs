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
        public string TongTien { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThai { get; set; }
        public int Id_NhanVien { get; set; }
        public int Id_KhachHang { get; set; }

    }
}
