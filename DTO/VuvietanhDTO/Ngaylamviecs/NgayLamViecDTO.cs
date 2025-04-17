using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Ngaylamviecs
{
    public class NgayLamViecDTO
    {
        public int Id { get; set; }
        public DateTime Ngay { get; set; } // Ngày cụ thể
        public bool IsNgayNghi { get; set; } = false; // Đánh dấu ngày nghỉ (mặc định là ngày làm việc)
        public string GhiChu { get; set; } = string.Empty; 
    }
}
