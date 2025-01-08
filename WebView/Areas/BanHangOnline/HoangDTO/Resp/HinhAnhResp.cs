namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class HinhAnhResp
    {
        public int? Id { get; set; }
        public string? Url { get; set; }
        public string? ImageData { get; set; }
        public int? Id_SanPham { get; set; }
        public int ImageSourceType { get; set; } = 1; // 0: File, 1: Link URL

    }
}
