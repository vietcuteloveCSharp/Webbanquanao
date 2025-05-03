using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class NhanVien
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? TaiKhoan { get; set; } // Cho phép null
        public string? MatKhau { get; set; } // Cho phép null
        public string TenNhanVien { get; set; } = string.Empty;
        public string Sdt { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; } = null;
        public string DiaChi { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; }

        [ForeignKey("ChucVu")]
        public int Id_ChucVu { get; set; }

        public virtual ChucVu ChucVu { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<CaNhanVien> Canhanviens { get; set; }
    }
}
