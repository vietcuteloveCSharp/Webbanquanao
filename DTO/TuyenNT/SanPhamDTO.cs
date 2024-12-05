using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.TuyenNT
{
    public class SanPhamDTO
    {
       
        public string Ten { get; set; } 
        public string MoTa { get; set; }
        public string Gia { get; set; }
        public int SoLuong { get; set; } 
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public bool TrangThai { get; set; } 
        public string HinhAnh { get; set; } 


        public int Id_ThuongHieu { get; set; }
  
        public int Id_DanhMuc { get; set; }
    }
}
