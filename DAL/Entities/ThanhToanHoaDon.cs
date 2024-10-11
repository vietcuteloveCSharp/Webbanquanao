using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ThanhToanHoaDon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double TongTien { get; set; }
        public double SoTienDaThanhToan { get; set; }
        public DateTime NgayThanhToan { get; set; } = DateTime.Now;
        public string MaGiaoDich { get; set; } = string.Empty;

        [ForeignKey("HoaDon")]
        public int Id_HoaDon { get; set; }
        [ForeignKey("PhuongThucThanhToan")]
        public int Id_PhuongThucThanhToan { get; set; }

        public virtual HoaDon HoaDon { get; set; }
        public virtual PhuongThucThanhToan PhuongThucThanhToan { get; set; }

    }
}
