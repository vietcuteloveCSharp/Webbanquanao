namespace WebView.NghiaDTO
{
    public class HinhAnhDTO
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? ImageData { get; set; }
        public int Id_SanPham { get; set; }
        public int ImageSourceType { get; set; } // 0: File, 1: Link URL
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
