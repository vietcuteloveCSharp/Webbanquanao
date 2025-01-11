using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enum.EnumVVA
{
    public enum ETrangThaiHD
    {
        // Trạng thái chung
        ChoXuLy = 0,           // Hóa đơn đang chờ xử lý
        HoanThanhDon = 5,      // Đơn hàng đã hoàn thành
        HuyDon = 7,            // Đơn hàng bị hủy

        // Trạng thái dành riêng cho bán online
        ChoXacNhan = 1,        // Đơn hàng chờ xác nhận từ người bán
        ChoThanhToan = 2,      // Đơn hàng chờ thanh toán (nếu không phải COD)
        DangVanChuyen = 3,     // Đơn hàng đang được vận chuyển (đã thanh toán trước)
        DangVanChuyenCOD = 4,  // Đơn hàng COD đang được vận chuyển
        HoanHang = 6,          // Đơn hàng đã được hoàn trả
        DaThanhToan = 8
    }
}
