using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resquests.AccountKhachhang
{
    //validate request
    public class RegisterKHResquest
    {
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "TaiKhoan không được chứa khoảng trắng.")]
        [Required(ErrorMessage = "TaiKhoan là bắt buộc.")]
        public string TaiKhoan { get; set; }
        [Required(ErrorMessage = "MatKhau là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [RegularExpression(@"^(?=\S*$)(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,50}$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất 1 ký tự viết hoa, 1 ký tự số và 1 ký tự đặc biệt.")]
        public string MatKhau { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; } = null;
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [Required(ErrorMessage = "Tên là bắt buộc")]
        public string Ten { get; set; } = string.Empty;
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [RegularExpression(@"^\+84\d{9}$", ErrorMessage = "Số điện thoại phải là 10 chữ số")]
        public string Sdt { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [MaxLength(50, ErrorMessage = "Email không được vượt quá 50 kí tự.")]
        public string Email { get; set; } = string.Empty;
        public bool TrangThai { get; set; } = true;
    }
}
