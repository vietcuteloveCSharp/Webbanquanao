using DAL.Context;
using Enum.EnumVVA;
using Microsoft.AspNetCore.Mvc;
using WebView.NghiaDTO;
using Microsoft.EntityFrameworkCore;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongKeController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public ThongKeController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> DoanhThu()
        {
            // Lấy danh sách các hóa đơn đã thanh toán (DaThanhToan = 8)
            var hoaDons = await _context.HoaDons
                .Where(hd => hd.TrangThai == ETrangThaiHD.DaThanhToan)
                .Select(hd => new HoaDonThongKeDTO
                {
                    Id = hd.Id,
                    TongTien = hd.TongTien,
                    PhiVanChuyen = hd.PhiVanChuyen,
                    Total = hd.TongTien - hd.PhiVanChuyen,
                    NgayTao = hd.NgayTao
                })
                .ToListAsync();

            // Tính tổng doanh thu
            var totalRevenue = hoaDons.Sum(hd => hd.Total);

            // Tạo mô hình dữ liệu để hiển thị
            var model = new DoanhThuViewModel
            {
                TotalRevenue = totalRevenue,
                HoaDons = hoaDons
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult ThongKeDoanhThuTheoNgay(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.HoaDons
                .Where(hd => hd.TrangThai == ETrangThaiHD.DaThanhToan);

            if (startDate.HasValue)
            {
                query = query.Where(hd => hd.NgayTao.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(hd => hd.NgayTao.Date <= endDate.Value.Date);
            }

            var hoaDons = query
                .Select(hd => new
                {
                    Id = hd.Id,
                    Total = hd.TongTien - hd.PhiVanChuyen,
                    NgayTao = hd.NgayTao
                })
                .ToList();

            var totalRevenue = hoaDons.Sum(hd => hd.Total);

            return Json(new
            {
                hoaDons,
                totalRevenue
            });
        }

    }
}