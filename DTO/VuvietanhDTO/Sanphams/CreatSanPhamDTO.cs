using DTO.VuvietanhDTO.ValidateCustomDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Sanphams
{
    public class CreatSanPhamDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên sản phẩm không được vượt quá 50 ký tự.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Tên không được chỉ chứa khoảng trắng.")]
        public string Ten { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự.")]
        public string MoTa { get; set; } = string.Empty;
        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Giá phải là số hợp lệ, tối đa 2 chữ số thập phân.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 1.")]
        public int SoLuong { get; set; } = 1;
        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Giá phải là số hợp lệ, tối đa 2 chữ số thập phân.")]
        public string Gia { get; set; }

        [Required(ErrorMessage = "Ngày tạo là bắt buộc.")]
        [NotInFuture(ErrorMessage = "Ngày tạo không được ở tương lai.")]
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true;
        [Required(ErrorMessage = "Hình ảnh là bắt buộc.")]
        [MaxLength(200, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 200 ký tự.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Đường dẫn hình ảnh không được chỉ chứa khoảng trắng.")]
        public string HinhAnh { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thương hiệu là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Thương hiệu không hợp lệ.")]
        public int Id_ThuongHieu { get; set; }

        [Required(ErrorMessage = "Danh mục là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Danh mục không hợp lệ.")]
        public int Id_DanhMuc { get; set; }
    }
}
