namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class SanPhamTimKiemResp
    {
        public ChiTietSanPhamResp ChiTietSanPhams { get; set; } = new ChiTietSanPhamResp();
        public DanhMucResp DanhMucs { get; set; } = new DanhMucResp();
    }
}
