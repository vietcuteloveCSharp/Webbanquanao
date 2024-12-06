using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO
{
    public class MauSacDTO
    {
        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Tên không được chứa chỉ khoảng trắng.")]
        public string Ten { get; set; } = string.Empty;
        [MaxLength(20, ErrorMessage = "Mã Hex không thể dài hơn 20 ký tự.")]
        [RegularExpression(@"^[A-Fa-f0-9]+$", ErrorMessage = "Mã Hex chỉ chứa các ký tự A-F và 0-9.")]
        public string MaHex { get; set; } = string.Empty;
    }
}
