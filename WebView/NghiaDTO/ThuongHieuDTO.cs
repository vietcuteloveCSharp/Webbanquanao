using System.ComponentModel.DataAnnotations;

namespace WebView.NghiaDTO
{
    public class ThuongHieuDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        public string Ten { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "không được vượt quá 50 kí tự")]
        public string MoTa { get; set; } = string.Empty;
        public bool TrangThai { get; set; }
    }
}
