using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Danhmucs
{
    public class FullDanhMucDTO
    {
        public int Id { get; set; }
        public int? Id_DanhMucCha { get; set; } = null; // Phân cấp danh mục dạng tree
        public string TenDanhMuc { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; }
    }
}
