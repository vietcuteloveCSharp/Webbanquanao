using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO
{
    //mã màu sắc
    public class MauSacDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Tên không được chứa chỉ khoảng trắng.")]
        public string Ten { get; set; } = string.Empty;
        [Required(ErrorMessage = "Max Hex là bắt buộc.")]
        [MaxLength(20, ErrorMessage = "Mã Hex không thể dài hơn 20 ký tự.")]
        [RegularExpression(@"^#?[A-Fa-f0-9]{6}$", ErrorMessage = "Mã Hex chỉ chứa ký tự A-F, 0-9 và có độ dài 6 ký tự.")]

        public string MaHex { get; set; } = string.Empty;
    }
}
