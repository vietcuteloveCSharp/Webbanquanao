namespace WebView.Areas.BanTaiQuay.DTOBanTaiQuay.Param
{
    public class ThanhToanTaiQuayParam
    {
        public string idkhachang { get; set; }
        public List<ChiTietSanPhamTaiQuay> lsthoadon { get; set; }
        public int tongtienhang { get; set; }
        public int tongtienthanhtoan { get; set; }
    }

    public class ChiTietSanPhamTaiQuay
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public int gia { get; set; } = 0;
    }
}
