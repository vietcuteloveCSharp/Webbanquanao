namespace DAL.DataSeed
{
    public static class EnumableClass
    {
        public enum GioiTinhEnum
        {
            Nam,
            Nu
        }
        public enum ChucVuEnum
        {
            QuanTriVien,
            ThuNgan,
            BanHang
        }

        public enum LoaiGiamGiaEnum
        {
            Coupon,
            Voucher,
        }
        public enum TrangThaiMaGiamGiaEnum
        {
            //0 - chưa phát hành || 1 - đang phát hành || 2 - kết thúc
            ChuaPhatHanh,
            DangPhatHanh,
            KetThuc
        }
        public enum TrangThaiCoBanEnum
        {
            //false - ngừng khuyến mại || true - đang khuyến mại
            NgungHoatDong,
            DangHoatDong,
        }
        public enum KhuyenMaiTrangThaiEnum
        {
            //false - ngừng khuyến mại || true - đang khuyến mại
            NgungkhuyenMai,
            DangKhuyenMai,
        }
        public enum TrangThaiHoaDonEnum
        {
            //false - ngừng khuyến mại || true - đang khuyến mại
            ChuaHoanThanh,
            DaHoanThanh,
        }
        
    }
}
