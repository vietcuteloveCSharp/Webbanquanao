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
       
      
        // Trạng thái dành riêng cho bán online
        ChoXacNhan = 1,        // Đơn hàng chờ xác nhận từ người bán
        ChoThanhToan = 2,      // Đơn hàng chờ thanh toán (nếu không phải COD)
        DaXacNhan = 3,     // Đơn hàng đang được vận chuyển (đã thanh toán trước)
        DangVanChuyen = 4,  // Đơn hàng  đang được vận chuyển
        HoanThanhDon = 5,      // Đơn hàng đã hoàn thành
        HuyDon = 6,            // Đơn hàng bị hủy
    }
}
