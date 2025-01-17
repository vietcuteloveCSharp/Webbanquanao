using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ChiTietHoaDon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public bool TrangThai { get; set; } = true;

        [ForeignKey("HoaDon")]
        public int Id_HoaDon { get; set; }
        [ForeignKey("ChiTietSanPham")]
        public int Id_ChiTietSanPham { get; set; }

        public virtual HoaDon? HoaDon { get; set; }
        public virtual ChiTietSanPham? ChiTietSanPham { get; set; }
    }
}
