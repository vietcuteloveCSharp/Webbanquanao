using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NTTuyen.ChiTietHoaDon
{
    public class FullChiTietHoaDonDTO
    {
        
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public string Gia { get; set; }
        public bool TrangThai { get; set; } = true;
        public int Id_HoaDon { get; set; }
        public int Id_ChiTietSanPham { get; set; }
    }
}
