using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class DanhMuc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Id_DanhMucCha { get; set; } = null; // Phân cấp danh mục dạng tree
        public string TenDanhMuc { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; }

        public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
