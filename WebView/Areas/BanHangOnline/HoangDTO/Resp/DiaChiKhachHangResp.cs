namespace WebView.Areas.BanHangOnline.HoangDTO.Resp
{
    public class DiaChiKhachHangResp
    {
        public int? Id { get; set; }
        public string? IdTinh { get; set; }  // mã tỉnh
        public string? TenTinh { get; set; }  /*tên tỉnh*/
        public string? IdQuan { get; set; }  /*mã quận*/
        public string? TenQuan { get; set; }  /*tên quận*/
        public string? IdPhuong { get; set; }  /*mã phường*/
        public string? TenPhuong { get; set; }  /*tên phường*/
        public string? ChiTietDiaChi { get; set; }  /*chi tiết địa chỉ*/
        public bool IsDefault { get; set; }  /*chọn làm mặc định*/
    }
}
