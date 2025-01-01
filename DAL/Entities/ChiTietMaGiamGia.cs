using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ChiTietMaGiamGia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Id_KhachHang { get; set; } = null;
        public int? Id_HoaDon { get; set; } = null;
        public string NoiDung { get; set; } = string.Empty;
        public decimal TongTien { get; set; } = 0; // tổng số tiền giảm trên hóa đơn
        public DateTime? NgaySuDung { get; set; } = null;

        [ForeignKey("MaGiamGia")]
        public int? Id_MaGiamGia { get; set; } = null;

        public virtual MaGiamGia MaGiamGia { get; set; }
    }
}
