namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class ChiTietSanPhamResp
    {
        public SanPhamResp? SanPham { get; set; }
        public List<MauSacResp>? MauSacResps { get; set; }
        public List<KichThuocResp>? KichThuocResps { get; set; }
        public ThuongHieuResp? ThuongHieuResps { get; set; }
    }

    public class MauSacResp
    {
        public int? Id { get; set; }
        public string? Ten { get; set; }
        public string? MaHex { get; set; } = string.Empty;
    }
    public class KichThuocResp
    {
        public int? Id { get; set; }
        public int? Id_MauSac { get; set; }
        public string? Ten { get; set; }
    }
    public class ThuongHieuResp
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
    }
}
