using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class KhuyenMai
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên khuyến mại không được để trống.")]
        public string Ten { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả không được để trống.")]
        public string MoTa { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Loại khuyến mại phải trong khoảng từ 0 đến 100.")]
        public int LoaiKhuyenMai { get; set; } // theo % 

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị giảm phải lớn hơn hoặc bằng 0.")]
        public decimal GiaTriGiam { get; set; } // % giảm

        [Range(0, double.MaxValue, ErrorMessage = "Điều kiện giảm giá phải lớn hơn hoặc bằng 0.")]
        public decimal DieuKienGiamGia { get; set; } // điều kiện giảm sản phẩm 

        [Required(ErrorMessage = "Ngày tạo không được để trống.")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime NgayKetThuc { get; set; }
        public int TrangThai { get; set; } // 0 - ngừng khuyến mại || 1 - đang khuyến mại || 2- Kết thúc


        public List<ChiTietKhuyenMai> chiTietKhuyenMais { get; set; } = new();
    }
}
