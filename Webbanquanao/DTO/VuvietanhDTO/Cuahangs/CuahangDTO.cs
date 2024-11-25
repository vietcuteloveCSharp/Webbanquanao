using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Cuahangs
{
    public class CuahangDTO
    {
        [Required(ErrorMessage = "Tên cửa hàng là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Không được vượt quá 50 kí tự")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Tên cửa hàng không được chứa chỉ khoảng trắng")]
        public string Ten { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ cửa hàng là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Không được vượt quá 50 kí tự")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Tên cửa hàng không được chứa chỉ khoảng trắng")]
        public string DiaChi { get; set; } = string.Empty;
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải là 10 chữ số và không chứa khoảng trắng.")]
        public string Sdt { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [MaxLength(50, ErrorMessage = "Email không được vượt quá 50 kí tự.")]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
