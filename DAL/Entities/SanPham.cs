using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class SanPham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public double Gia { get; set; } = 0;
        public int SoLuong { get; set; } = 1;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; }
        public bool TrangThai { get; set; } = true;
        public string HinhAnh { get; set; } = string.Empty;

        [ForeignKey("ThuongHieu")]
        public int Id_ThuongHieu { get; set; }
        [ForeignKey("DanhMuc")]
        public int Id_DanhMuc { get; set; }

        public virtual ThuongHieu ThuongHieu { get; set; }
        public virtual DanhMuc DanhMuc { get; set; }
        public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }

    }
}
