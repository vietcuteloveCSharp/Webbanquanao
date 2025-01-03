using DAL.Entities;

namespace WebView.NghiaDTO
{
    public class KhuyenMaiDTO
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public int LoaiKhuyenMai { get; set; } // theo % 
        public decimal GiaTriGiam { get; set; } // % giảm
        public decimal DieuKienGiamGia { get; set; } // điều kiện giảm sản phẩm 
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public bool TrangThai { get; set; } // false - ngừng khuyến mại || true - đang khuyến mại
        public int Id_DanhMuc { get; set; }
        public List<ChiTietKhuyenMaiDTO> ChiTietKhuyenMaiDTOs { get; set; }
    }
}
