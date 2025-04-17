using DTO.VuvietanhDTO.NhanViens;

namespace WebView.Areas.Admin.ViewModels
{
    public class DayWorkViewModel
    {
        public int Id { get; set; }
        public DateTime Ngay { get; set; }
        public bool IsNgayNghi { get; set; }
        public string GhiChu { get; set; }
        public List<ShiftView> ShiftViews { get; set; }
    }
    public class ShiftView
    {
        public int Id { get; set; }
        public Enum.EnumVVA.EnumTenCa TenCa { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public List<NhanVienProfileDTO> NhanViens { get; set; }
    }
}
