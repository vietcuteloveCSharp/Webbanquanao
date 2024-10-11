using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class HoaDon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double TongTien { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public int TrangThai { get; set; }

        [ForeignKey("NhanVien")]
        public int Id_NhanVien { get; set; }
        [ForeignKey("KhachHang")]
        public int Id_KhachHang { get; set; }

        public virtual NhanVien NhanVien { get; set; }
        public virtual KhachHang KhachHang { get; set; }

        public virtual ICollection<ThanhToanHoaDon> ThanhToanHoaDons { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    }
}
