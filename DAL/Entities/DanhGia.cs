using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class DanhGia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Id_ChiTietHoaDon { get; set; }
        public int Id_KhachHang { get; set; }
        public int Sao { get; set; } = 5;// số sao đánh giá từ 1 -> 5. mặc định 5 sao
        public string? NoiDung { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public int TrangThai { get; set; } = 1; // 0: ẩn || 1: hiện
    }
}
