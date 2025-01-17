using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebView.NghiaDTO
{
    public class KhuyenMaiDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên khuyến mại không được để trống.")]
        public string Ten { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả không được để trống.")]
        public string MoTa { get; set; } = string.Empty;

        [Required(ErrorMessage = "% giảm giá không được để trống.")]

        [Range(1, 100, ErrorMessage = "Giá trị % giảm giá  phải trong khoảng từ 1% đến 100%.")]

        public decimal GiaTriGiam { get; set; } // % giảm
        [Required(ErrorMessage = "Điều kiện giảm giá  không được để trống.")]
        [Range(1, 20000000, ErrorMessage = "Điều kiện giảm giá không được vượt quá 20 triệu")]


        public decimal DieuKienGiamGia { get; set; } // điều kiện giảm sản phẩm 

        public DateTime NgayTao { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime NgayKetThuc { get; set; }
        public int TrangThai { get; set; } // 0 - ngừng khuyến mại || 1 - đang khuyến mại || 2- Kết thúc
                                           //public int Id_DanhMuc { get; set; }
        public List<ChiTietKhuyenMaiDTO> chiTietKhuyenMaiDTOs { get; set; } = new();
        public List<SelectListItem> DanhMucs { get; set; }
        // Thêm thuộc tính này để lưu danh sách ID danh mục đã chọn
    }
}
