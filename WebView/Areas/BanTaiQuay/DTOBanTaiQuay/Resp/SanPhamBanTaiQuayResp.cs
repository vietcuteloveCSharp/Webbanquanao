using WebView.Areas.BanHangOnline.HoangDTO.Resp;

namespace WebView.Areas.BanTaiQuay.DTOBanTaiQuay.Resp
{
    public class SanPhamBanTaiQuayResp
    {
        public int? Id { get; set; }
        public int? Id_DanhMuc { get; set; }
        public string? Ten { get; set; }
        public string? MoTa { get; set; }
        public decimal GiaBan { get; set; } = 0;
        public decimal GiaBanDau { get; set; } = 0;
        public int? SoLuong { get; set; } = 1; // số lượng của sản phẩm chi tiết
        public MauSacResp MauSac { get; set; } = new MauSacResp();
        public KichThuocResp KichThuoc { get; set; } = new KichThuocResp();
        public List<HinhAnhResp>? ListHinHAnh { get; set; }
    }
}
