    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ChiTietSanPham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true;

        [ForeignKey("SanPham")]
        public int Id_SanPham { get; set; }
        [ForeignKey("MauSac")]
        public int Id_MauSac { get; set; }
        [ForeignKey("KichThuoc")]
        public int Id_KichThuoc { get; set; }

        public virtual SanPham SanPham { get; set; }
        public virtual MauSac MauSac { get; set; }
        public virtual KichThuoc KichThuoc { get; set; }

        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
