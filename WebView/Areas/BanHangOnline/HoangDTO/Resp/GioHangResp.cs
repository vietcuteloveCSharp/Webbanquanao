namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class GioHangResp
    {
        public int? Id { get; set; }
        public int? SoLuong { get; set; }
        public int? IdChiTietSanPham { get; set; }
        public int? IdKichThuoc { get; set; }
        public int? IdMauSac { get; set; }
        public SanPhamResp? SanPham { get; set; }
        public List<MauSacResp>? MauSacResps { get; set; }
        public List<KichThuocResp>? KichThuocResps { get; set; }
    }
}
