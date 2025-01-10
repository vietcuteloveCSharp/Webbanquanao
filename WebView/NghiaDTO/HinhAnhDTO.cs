using System.ComponentModel.DataAnnotations;

namespace WebView.NghiaDTO
{
    public class HinhAnhDTO
    {
        public int Id { get; set; } // Không được âm

        [Url(ErrorMessage = "Định dạng URL không hợp lệ.")]
        [Required(ErrorMessage = "Định dạng không được bỏ trống.")]

        public string? Url { get; set; } // Kiểm tra định dạng URL hợp lệ

        public string? ImageData { get; set; } // Kiểm tra dữ liệu hình ảnh hợp lệ (dữ liệu base64 hoặc định dạng khác)

        [Required(ErrorMessage = "ID sản phẩm không được bỏ trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "ID sản phẩm phải là một số không âm.")]
        public int Id_SanPham { get; set; } // Không được âm

        [Required(ErrorMessage = "Loại nguồn hình ảnh không được bỏ trống.")]
        [Range(0, 1, ErrorMessage = "Loại nguồn hình ảnh phải là 0 (File) hoặc 1 (Link URL).")]
        public int ImageSourceType { get; set; } // 0: File, 1: Link URL

        public DateTime NgayTao { get; set; } = DateTime.Now; // Đảm bảo là một ngày hợp lệ
    }
}
