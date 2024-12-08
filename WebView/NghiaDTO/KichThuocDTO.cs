using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO
{
    public class KichThuocDTO
    {
        [Required(ErrorMessage = "Tên kích thước là bắt buộc.")]
        [MaxLength(20, ErrorMessage = "Không vượt quá 20 kí tự")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Tên không được chỉ chứa khoảng trắng.")]
        public string Ten { get; set; } = string.Empty;
    }
}
