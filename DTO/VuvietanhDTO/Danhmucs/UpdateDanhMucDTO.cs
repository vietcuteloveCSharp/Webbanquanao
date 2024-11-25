using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Danhmucs
{
    public class UpdateDanhMucDTO
    {
        public int? Id_DanhMucCha { get; set; } = null; // Phân cấp danh mục dạng tree
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Tên danh mục không được chỉ chứa khoảng trắng.")]
        public string TenDanhMuc { get; set; } = string.Empty;
        public bool TrangThai { get; set; }
    }
}
