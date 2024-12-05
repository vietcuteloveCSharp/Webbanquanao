using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class GioHang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public bool TrangThai { get; set; } = true;

        [ForeignKey("KhachHang")]
        public int Id_KhachHang { get; set; }
        [ForeignKey("ChiTietSanPham")]
        public int Id_ChiTietSanPham { get; set; }

        public virtual KhachHang KhachHang { get; set; }
        public virtual ChiTietSanPham ChiTietSanPham { get; set; }
    }
}
