using System.ComponentModel.DataAnnotations;

namespace WebView.NghiaDTO
{
    public class SoLuongSanPhamDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu không được bỏ trống số lượng")]
        public int SoLuong { get; set; }

        public int SanPhamId { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
