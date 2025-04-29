using System.ComponentModel.DataAnnotations;

namespace WebView.Areas.BanHangOnline.HoangDTO.Param
{
    public class DangKyParam
    {
        [Required]
        public string TaikhoanDK { get; set; } = string.Empty;
        [Required]
        public string NameFullDK { get; set; } = string.Empty;
        [Required]
        public string RegisterPhoneDK { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string EmailDK { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime? NgaySinhDK { get; set; } = null;
        [Required]

        public string RegisterPasswordDK { get; set; } = string.Empty;
        [Required]
        public string ConfirmPasswordDK { get; set; } = string.Empty;
    }
}
