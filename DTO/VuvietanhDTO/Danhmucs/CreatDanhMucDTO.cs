using DTO.VuvietanhDTO.ValidateCustomDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Danhmucs
{
    public class CreatDanhMucDTO
    {
        public int? Id_DanhMucCha { get; set; } = null; // Phân cấp danh mục dạng tree
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Tên danh mục không được chỉ chứa khoảng trắng.")]
        public string TenDanhMuc { get; set; } = string.Empty;
        [Required(ErrorMessage = "Ngày tạo là bắt buộc.")]
        [NotInFuture(ErrorMessage = "Ngày tạo không được ở tương lai.")]
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
