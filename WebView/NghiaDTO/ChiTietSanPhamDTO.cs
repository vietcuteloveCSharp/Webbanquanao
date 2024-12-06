using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO{
    public class ChiTietSanPhamDTO
    {
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true;

        public int Id_SanPham { get; set; }
        public int Id_MauSac { get; set; }
        public int Id_KichThuoc { get; set; }
    }

}
