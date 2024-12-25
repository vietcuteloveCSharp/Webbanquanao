using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebView.NghiaDTO
{
    public class ChiTietMaGiamGiaDTO
    {
        public int Id { get; set; }
        public int? Id_KhachHang { get; set; } = null;
        public string NoiDung { get; set; } = string.Empty;
        public DateTime? NgaySuDung { get; set; } = null;

        [ForeignKey("MaGiamGia")]
        public int? Id_MaGiamGia { get; set; } = null;

        public virtual MaGiamGiaDTO MaGiamGiaDTOs { get; set; }
    }
}
