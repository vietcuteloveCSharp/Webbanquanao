using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Sanphams
{
    public class FullSanPhamDTO
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public int SoLuong { get; set; } = 1;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; }
        public bool TrangThai { get; set; } = true;
        public string HinhAnh { get; set; } = string.Empty;

        public int Id_ThuongHieu { get; set; }
        public int Id_DanhMuc { get; set; }
    }
}
