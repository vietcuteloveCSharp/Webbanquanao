namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class DanhGiaSanPhamResp
    {
        public int IdChiTietHoaDon { get; set; } // Id của chi tiết hóa đơn
        public ChiTietSanPhamResp ChiTietSanPham { get; set; }
        public bool TrangThai { get; set; } // false: chưa đánh giá || true: đã đánh giá
        public int Sao { get; set; } // Số sao đã đánh giá
        public string? NoiDung { get; set; }
        public List<int> HinhAnhs { get; set; } = null;
    }
}
