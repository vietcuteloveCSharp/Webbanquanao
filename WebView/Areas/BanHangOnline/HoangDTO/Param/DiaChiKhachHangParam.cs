namespace WebView.Areas.BanHangOnline.HoangDTO.Param
{
    public class DiaChiKhachHangParam
    {
        public string IdTinh { get; set; } = null!; // mã tỉnh
        public string TenTinh { get; set; } = null!; /*tên tỉnh*/
        public string IdQuan { get; set; } = null!; /*mã quận*/
        public string TenQuan { get; set; } = null!; /*tên quận*/
        public string IdPhuong { get; set; } = null!; /*mã phường*/
        public string TenPhuong { get; set; } = null!; /*tên phường*/
        public string ChiTietDiaChi { get; set; } = null!; /*chi tiết địa chỉ*/
        public bool IsDefault { get; set; } = false; /*chọn làm mặc định*/
    }
}
