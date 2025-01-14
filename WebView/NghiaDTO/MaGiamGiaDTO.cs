using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebView.NghiaDTO
{
    public class MaGiamGiaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống.")]
        [DataType(DataType.Text)]
        public string Ten { get; set; }

        [Range(0, 1, ErrorMessage = "Loại giảm giá phải là 0 hoặc 1.")]
        public int LoaiGiamGia { get; set; } // 0 - coupon || 1 - voucher

        [Range(1, double.MaxValue, ErrorMessage = "Điều kiện giảm giá phải phải > 0")]
        public decimal? DieuKienGiamGia { get; set; } // > 0 thì yêu cầu đơn hàng trên dk thì mới áp đc || = 0 thì không cần check đơn hàng

        [Range(0, 100, ErrorMessage = "Giá trị giảm phải nằm trong khoảng từ 0 đến 100.")]

        public decimal? GiaTriGiam { get; set; } // GiaTriGiam sẽ là %: 10, 20, 30 %

        [Range(1, double.MaxValue, ErrorMessage = "Mệnh giá phải phải > 0.")]
        public decimal? MenhGia { get; set; } // Giá trị của mã giảm giá: 20000 vnd, 50000 vnd

        public string NoiDung { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "Giá trị tối đa phải > 0")]
        public decimal? GiaTriToiDa { get; set; } // Giá trị tối đa của mã giảm giá

        [Range(0, 2, ErrorMessage = "Trạng thái phải là 0, 1 hoặc 2.")]
        public int TrangThai { get; set; } // 0 - chưa phát hành || 1 - đang phát hành || 2 - kết thúc

        public DateTime ThoiGianTao { get; set; } = DateTime.Now;

        public DateTime? ThoiGianKetThuc { get; set; } // Ngày kết thúc

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0.")]
        public int? SoLuong { get; set; } // Số lượng mã giảm giá

  
        public int? SoLuongDaSuDung { get; set; } // Số lượng mã giảm giá đã sử dụng

        public virtual ICollection<ChiTietMaGiamGiaDTO> ChiTietMaGiamGiaDTOs { get; set; }
    }
}
