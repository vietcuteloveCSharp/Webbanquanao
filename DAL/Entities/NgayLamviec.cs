using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class NgayLamViec
    {
        public int Id { get; set; }
        public DateTime Ngay { get; set; } // Ngày cụ thể
        public bool IsNgayNghi { get; set; } = false; // Đánh dấu ngày nghỉ (mặc định là ngày làm việc)
        public string GhiChu { get; set; } = string.Empty; // Lưu thông tin đặc biệt về ngày đó
                                                          // Liên kết với bảng CaLamViec
        public virtual ICollection<CaLamViec> CaLamViecs{ get; set; }
    }
}
