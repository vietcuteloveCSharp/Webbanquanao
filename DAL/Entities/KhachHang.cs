using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class KhachHang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TaiKhoan { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; } = null;
        public string Ten { get; set; } = string.Empty;
        public string Sdt { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool TrangThai { get; set; } = true;

        public virtual ICollection<GioHang>? GioHangs { get; set; } = new List<GioHang>();
        public virtual ICollection<HoaDon>? HoaDons { get; set; } = new List<HoaDon>();
    }
}
