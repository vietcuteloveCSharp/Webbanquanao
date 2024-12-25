namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class SanPhamResp
    {
        public int? Id { get; set; }
        public int? Id_DanhMuc { get; set; }
        public string? Ten { get; set; }
        public string? MoTa { get; set; }
        public decimal GiaBan { get; set; } = 0;
        public decimal GiaBanDau { get; set; } = 0;
        public int? SoLuong { get; set; } = 1;
        public List<HinhAnhResp>? ListHinHAnh { get; set; }
    }

    public class DictionarySanPhamResp
    {
        public Dictionary<DanhMucResp, List<SanPhamResp>> DictionarySanPham { get; set; }
    }
}
