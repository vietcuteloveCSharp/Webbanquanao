namespace WebView.NghiaDTO
{
    public class DoanhThuViewModel
    {
        public decimal TotalRevenue { get; set; }
        public List<HoaDonThongKeDTO> HoaDons { get; set; } = new List<HoaDonThongKeDTO>();
    }
}
