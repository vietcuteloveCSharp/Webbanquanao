﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class MaGiamGia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Ten { get; set; }
        public int LoaiGiamGia { get; set; } // 0 - coupon || 1 - voucher
        public decimal DieuKienGiamGia { get; set; }// > 0 thì yêu cầu đơn hàng trên dk thì mới áp đc MaGiamGia || = 0 thì ko cần check đơn hàng
        public decimal? GiaTriGiam { get; set; } // GiaTriGiam sẽ là %. 10, 20, 30 % đơn hàng
        public decimal MenhGia { get; set; } // Giá trị của MaGiamGia. 20000 vnd, 50000 vnd
        public string NoiDung { get; set; } = string.Empty;
        public decimal? GiaTriToiDa { get; set; } // Giá trị tối đa của mã giảm giá
        public int TrangThai { get; set; } // 0 - chưa phát hành || 1 - đang phát hành || 2 - kết thúc
        public DateTime ThoiGianTao { get; set; } = DateTime.Now;
        public DateTime? ThoiGianKetThuc { get; set; }  // Ngày kết thúc
        public int? SoLuong { get; set; } // Số lượng mã giảm giá
        public int? SoLuongDaSuDung { get; set; } // Số lượng mã giảm giá
        public virtual ICollection<ChiTietMaGiamGia> ChiTietMaGiamGias { get; set; }
    }
}
