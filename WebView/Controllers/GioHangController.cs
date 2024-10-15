using DAL.Context;
using DAL.Entities;
using DAL.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Controllers
{
    public class GioHangController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public GioHangController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            var gioHangItems = await _context.GioHangs
                .Include(g => g.ChiTietSanPham)
                .Where(g => g.Id_KhachHang == khachHangId && g.TrangThai)
                .ToListAsync();

            return View(gioHangItems);
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int chiTietSanPhamId, int soLuong)
        {
            try
            {
                var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

                // Nếu người dùng chưa đăng nhập, lưu vào cookie
                if (khachHangId == null)
                {
                    var cookieCart = Request.Cookies.GetObjectFromJson<List<GioHang>>("GioHangs");
                    if (cookieCart == null)
                    {
                        cookieCart = new List<GioHang>();
                    }

                    var existingItem = cookieCart.FirstOrDefault(c => c.Id_ChiTietSanPham == chiTietSanPhamId);
                    if (existingItem != null)
                    {
                        existingItem.SoLuong += soLuong;
                    }
                    else
                    {
                        cookieCart.Add(new GioHang { Id_ChiTietSanPham = chiTietSanPhamId, SoLuong = soLuong });
                    }

                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                        IsEssential = true
                    };
                    Response.Cookies.SetObjectAsJson("GioHang", cookieCart, cookieOptions);

                    return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng" });
                }

                // Nếu người dùng đã đăng nhập, lưu vào cơ sở dữ liệu
                var existingItemDb = _context.GioHangs
                    .FirstOrDefault(g => g.Id_ChiTietSanPham == chiTietSanPhamId && g.Id_KhachHang == khachHangId);

                if (existingItemDb != null)
                {
                    existingItemDb.SoLuong += soLuong;
                    _context.GioHangs.Update(existingItemDb);
                }
                else
                {
                    var newGioHangItem = new GioHang
                    {
                        Id_KhachHang = khachHangId.Value,
                        Id_ChiTietSanPham = chiTietSanPhamId,
                        SoLuong = soLuong,
                        TrangThai = true
                    };
                    _context.GioHangs.Add(newGioHangItem);
                }

                _context.SaveChanges();
                return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng" });
            }
            catch (Exception ex)
            {
                // Log lỗi ra console hoặc file để kiểm tra
                Console.WriteLine($"Lỗi: {ex.Message}");
                return Json(new { success = false, message = "Đã có lỗi xảy ra. Vui lòng thử lại sau!" });
            }
        }



        // Xóa sản phẩm khỏi giỏ hàng
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var gioHangItem = await _context.GioHangs.FindAsync(id);
            if (gioHangItem != null)
            {
                _context.GioHangs.Remove(gioHangItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public async Task<IActionResult> UpdateQuantity(int id, int soLuong)
        {
            var gioHangItem = await _context.GioHangs.FindAsync(id);
            if (gioHangItem != null)
            {
                gioHangItem.SoLuong = soLuong;
                _context.GioHangs.Update(gioHangItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }

}
