using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.TuyenNT
{
    public class DanhMucDTO
    {
        public int? Id_DanhMucCha { get; set; } 
        public string TenDanhMuc { get; set; } 
        public DateTime NgayTao { get; set; } 
        public bool TrangThai { get; set; }
    }
}
