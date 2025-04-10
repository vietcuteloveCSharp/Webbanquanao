using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebView.NghiaDTO
{
    public class NhanVienDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [StringLength(50, ErrorMessage = "Tài khoản không được vượt quá 50 ký tự")]
        public string? TaiKhoan { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 7, ErrorMessage = "Mật khẩu phải dài hơn 6 ký tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{7,}$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái thường, một chữ cái hoa, một số và một ký tự đặc biệt (!@#$%^&*)")]
        public string? MatKhau { get; set; }
        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [StringLength(100, ErrorMessage = "Tên nhân viên không được vượt quá 100 ký tự")]
        public string TenNhanVien { get; set; } = string.Empty;
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải đúng 10 chữ số")]
        [RegularExpression(@"^(0[0-9]{9,10})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Sdt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; } = null;
        public string DiaChi { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; }

        [ForeignKey("ChucVu")]
        public int Id_ChucVu { get; set; }

        public virtual ChucVuDTO ChucVuDTO{ get; set; }

        public virtual ICollection<HoaDonDTO> HoaDonDTOs{ get; set; }
    }
}
