namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class HoaDonChiTietResp
    {
        public decimal TongTien { get; set; } = 0;// tổng tiền = sản phẩm + Phí vận chuyển - tiền từ mã giảm giá
        public decimal PhiVanChuyen { get; set; } = 0; // tiền phí vận chuyển
        public string DiaChiGiaoHang { get; set; } = string.Empty;
        public DateTime NgayMua { get; set; }
        public string TrangThai { get; set; } // trạng thái đơn hàng
        public string TenKhachHang { get; set; } // tên khách hàng
        public string SDT { get; set; } // số điện thoại khách hàng
        // Danh sách chi tiết sản phẩm trong hóa đơn
        public List<HoaDonSanPhamChiTietResp> SanPhamResp { get; set; }
        // Hình thức mua hàng
        public string HinhThucMuaHang { get; set; } // vnpay || cod || taiquay
        public string PhuongThucThanhToan { get; set; } // Online || Offline
        public string HinhThucGiaoHang { get; set; } // Giao hàng tận nơi || Tại quầy
    }
}
