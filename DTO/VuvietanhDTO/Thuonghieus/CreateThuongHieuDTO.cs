using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Thuonghieus
{
    public class CreateThuongHieuDTO
    {
        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Tên không được chứa chỉ khoảng trắng.")]
        public string Ten { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mô tả không được chứa chỉ khoảng trắng.")]
        public string MoTa { get; set; } = string.Empty;
    }
}
