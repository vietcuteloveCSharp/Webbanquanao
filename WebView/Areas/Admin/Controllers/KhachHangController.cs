using DAL.Context;
using DTO.VuvietanhDTO.KhachHangs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public KhachHangController(WebBanQuanAoDbContext dbcontext)
        {
            _context = dbcontext;
          
        }
        // Hiển thị danh sách khách hàng
        public async Task<IActionResult> Index()
        {
            var khachHangs = await _context.KhachHangs
                .Select(kh => new KhachHangDTO
                {
                    Id = kh.Id,
                    TaiKhoan = kh.TaiKhoan,
                    Ten = kh.Ten,
                    Email = kh.Email,
                    Sdt = kh.Sdt,
                    Avatar = kh.Avatar,
                    TrangThai = kh.TrangThai
                }).ToListAsync();

            return View(khachHangs);
        }
        // Xem chi tiết khách hàng
        public async Task<IActionResult> Details(int id)
        {
            var khachHang = await _context.KhachHangs
                .Select(kh => new KhachHangDTO
                {
                    Id = kh.Id,
                    TaiKhoan = kh.TaiKhoan,
                    MatKhau = kh.MatKhau,
                    Ten = kh.Ten,
                    Email = kh.Email,
                    Sdt = kh.Sdt,
                    Avatar = kh.Avatar,
                    TrangThai = kh.TrangThai,
                    NgaySinh = kh.NgaySinh

                })
                .FirstOrDefaultAsync(kh => kh.Id == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }
        // Chuyển đổi trạng thái khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound();
            }

            // Chuyển đổi trạng thái
            khachHang.TrangThai = !khachHang.TrangThai;

            try
            {
                _context.KhachHangs.Update(khachHang);
                await _context.SaveChangesAsync();

                TempData["success"] = "Thay đổi trạng thái thành công!";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
