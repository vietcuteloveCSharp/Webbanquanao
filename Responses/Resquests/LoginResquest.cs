using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responses.Resquests
{
    public class LoginResquest
    {
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [MinLength(8, ErrorMessage = "Tài khoản phải có ít nhất 8 ký tự.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Tài khoản không được chứa chỉ khoảng trắng.")]
        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        [DefaultValue("Vuvietanh1")]
        public string TaiKhoan { get; set; } = null!;
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Không được vượt quá 50 kí tự")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        //[RegularExpression(@"^(?=\S*$)(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,50}$",
        //ErrorMessage = "Mật khẩu phải chứa ít nhất 1 ký tự viết hoa, 1 ký tự số và 1 ký tự đặc biệt.")]
        [DefaultValue("Vuvietanh1!")]

        public string MatKhau { get; set; } = null!;
    }
}
