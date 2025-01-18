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


        [Required(ErrorMessage = "Điều kiện giảm giá  không được để trống.")]
        [Range(1, 20000000, ErrorMessage = "Điều kiện giảm giá không được vượt quá 20 triệu")]


        public decimal? DieuKienGiamGia { get; set; } // > 0 thì yêu cầu đơn hàng trên dk thì mới áp đc || = 0 thì không cần check đơn hàng


        //[Required(ErrorMessage = "% giảm giá không được để trống.")]

        [Range(1, 100, ErrorMessage = "Giá trị % giảm giá  phải trong khoảng từ 1% đến 100%.")]

        public decimal? GiaTriGiam { get; set; } // GiaTriGiam sẽ là %: 10, 20, 30 %
       


        [Range(1, 20000000, ErrorMessage = "Mệnh giá không được bé hơn 1 và không quá 20 triệu")]

        public decimal? MenhGia { get; set; } // Giá trị của mã giảm giá: 10000 vnd, 5000000 vnd
        [Required(ErrorMessage = "Nội dung không được để trống.")]

        public string NoiDung { get; set; } = string.Empty;

        [Range(1, 10000000, ErrorMessage = "Giá trị tối đa không được vượt quá 10 triệu")]

        public decimal? GiaTriToiDa { get; set; } // Giá trị tối đa của mã giảm giá

        [Range(0, 2, ErrorMessage = "Trạng thái phải là 0, 1 hoặc 2.")]
        public int TrangThai { get; set; } // 0 - chưa phát hành || 1 - đang phát hành || 2 - kết thúc

        public DateTime ThoiGianTao { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Thời gian kết thúc không được để trống.")]

        public DateTime? ThoiGianKetThuc { get; set; } // Ngày kết thúc

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0.")]
        public int? SoLuong { get; set; } // Số lượng mã giảm giá

  
        public int? SoLuongDaSuDung { get; set; } // Số lượng mã giảm giá đã sử dụng
        public bool IsApplied { get; set; } // Thêm thuộc tính này

        public virtual ICollection<ChiTietMaGiamGiaDTO> ChiTietMaGiamGiaDTOs { get; set; }
        // Validation tuỳ chỉnh
        public bool ValidateCouponOrVoucher()
        {
            if (LoaiGiamGia == 0) // Coupon
            {
                // Kiểm tra các trường liên quan đến Coupon
                if (!GiaTriGiam.HasValue || !GiaTriToiDa.HasValue)
                {
                    return false;
                }
            }
            else if (LoaiGiamGia == 1) // Voucher
            {
                // Kiểm tra các trường liên quan đến Voucher
                if (!MenhGia.HasValue)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
