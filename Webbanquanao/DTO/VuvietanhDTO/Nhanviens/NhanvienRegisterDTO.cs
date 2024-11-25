using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.NhanViens
{
    public class NhanvienRegisterDTO
    {
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        [MinLength(8, ErrorMessage = "Tài khoản phải có ít nhất 8 ký tự.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Tài khoản không được chứa chỉ khoảng trắng.")]
        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        public string TaiKhoan { get; set; } = null!;
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Không được vượt quá 50 kí tự")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [RegularExpression(@"^(?=\S*$)(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,50}$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất 1 ký tự viết hoa, 1 ký tự số và 1 ký tự đặc biệt.")]

        public string MatKhau { get; set; } = null!;
        [Required(ErrorMessage = "Tên nhân viên là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Không được vượt quá 50 kí tự")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Tên nhân viên không được chứa chỉ khoảng trắng")]
        public string TenNhanVien { get; set; } = string.Empty;
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải là 10 chữ số và không chứa khoảng trắng.")]
        public string Sdt { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [MaxLength(50, ErrorMessage = "Email không được vượt quá 50 kí tự.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; } = null;
        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [MinLength(10, ErrorMessage = "Địa chỉ không được rỗng hoặc chỉ chứa khoảng trắng.")]
        [MaxLength(100, ErrorMessage = "Địa chỉ không được vượt quá 100 ký tự.")]
        public string DiaChi { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Chức vụ là bắt buộc")]
        public int Id_ChucVu { get; set; }
    }
}
