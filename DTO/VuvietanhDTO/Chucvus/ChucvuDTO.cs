using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Chucvus
{
    public class ChucvuDTO
    {
        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        public string Ten { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [MaxLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự.")]
        public string Mota { get; set; } = string.Empty;
    }
}
