﻿namespace WebView.Areas.BanHangOnline.HoangDTO
{
    public class SessionThanhToanModel
    {
        public string VnpTxnRef { get; set; }
        public int IdHoaDon { get; set; }
        public string PhuongThucThanhToan { get; set; } = "vnpay";
    }
}
