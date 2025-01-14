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
                .Where(hd => hd.TrangThai == ETrangThaiHD.HoanThanhDon)
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
                .Where(hd => hd.TrangThai == ETrangThaiHD.HoanThanhDon);

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
        [HttpGet]
        public IActionResult ThongKeSanPhamBanChay(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.ChiTietHoaDons
                .Include(cthd => cthd.ChiTietSanPham)
                .Include(cthd => cthd.HoaDon)
                .Where(cthd => cthd.HoaDon.TrangThai == ETrangThaiHD.HoanThanhDon);

            // Lấy top 3 sản phẩm bán chạy
            var sanPhamBanChay = query
                .GroupBy(cthd => new
                {
                    cthd.Id_ChiTietSanPham,
                    cthd.ChiTietSanPham.SanPham.Ten
                })
                .Select(g => new
                {
                    SanPhamId = g.Key.Id_ChiTietSanPham,
                    TenSanPham = g.Key.Ten,
                    TongSoLuong = g.Sum(cthd => cthd.SoLuong),
                })
                .OrderByDescending(sp => sp.TongSoLuong) // Sắp xếp giảm dần
                .Take(3) // Lấy top 3
                .ToList();

            return Json(new { sanPhamBanChay });
        }
      


    }
}