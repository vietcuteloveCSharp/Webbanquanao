using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class SanPham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        public string Ten { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        public string MoTa { get; set; } = string.Empty;
        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương.")]
        public decimal Gia { get; set; }
        public int SoLuong { get; set; } = 1;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; }
        public bool TrangThai { get; set; } = true;
        public string HinhAnh { get; set; } = string.Empty;

        [ForeignKey("ThuongHieu")]
        [Required(ErrorMessage = "Thương hiệu là bắt buộc.")]
        public int Id_ThuongHieu { get; set; }
        [ForeignKey("DanhMuc")]
        [Required(ErrorMessage = "Danh mục là bắt buộc.")]
        public int Id_DanhMuc { get; set; }

        public virtual ThuongHieu ThuongHieu { get; set; }
        public virtual DanhMuc DanhMuc { get; set; }
        public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }

        [NotMapped]
        [FileExtensions]

        public IFormFile? ImageUpload { get; set; }
    }
}
