namespace WebView.Areas.BanTaiQuay.DTOBanTaiQuay.Param
{
    public class PhuongThucThanhToanModel
    {
        public string Ten { get; set; } = string.Empty;
        public string Mota { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool TrangThai { get; set; } = true;
    }
}
