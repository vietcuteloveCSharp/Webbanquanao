using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.FilterDataDTO
{
    public class SanPhamFilter
    {
        [StringLength(255, ErrorMessage = "Từ khóa không được vượt quá 255 ký tự.")]
        public string? Keyword { get; set; } // Tìm kiếm theo tên hoặc mô tả

        [StringLength(255, ErrorMessage = "Tên thương hiệu không được vượt quá 255 ký tự.")]
        public string? TenThuongHieu { get; set; } // Lọc theo thương hiệu

        [StringLength(255, ErrorMessage = "Tên danh mục không được vượt quá 255 ký tự.")]
        public string? TenDanhMuc { get; set; } // Lọc theo tên danh mục

        [Range(0, double.MaxValue, ErrorMessage = "Giá tối thiểu không được nhỏ hơn 0.")]
        public decimal? MinGia { get; set; } // Giá tối thiểu

        [Range(0, double.MaxValue, ErrorMessage = "Giá tối đa không được nhỏ hơn 0.")]
        public decimal? MaxGia { get; set; } // Giá tối đa
        // Validate kiểm tra nếu MinGia > MaxGia, sẽ trả về lỗi
        public bool IsMinMaxValid => !(MinGia.HasValue && MaxGia.HasValue && MinGia > MaxGia);

        public bool? TrangThai { get; set; } // Lọc trạng thái sản phẩm
    }
}
