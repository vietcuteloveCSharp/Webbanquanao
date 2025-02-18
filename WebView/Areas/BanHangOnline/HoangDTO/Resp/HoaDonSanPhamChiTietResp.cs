namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class HoaDonSanPhamChiTietResp
    {
        // thông tin sản phẩm
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public string HinhAnh { get; set; } = string.Empty;
        public MauSacResp MauSac { get; set; }
        public KichThuocResp KichThuoc { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
    }
}
