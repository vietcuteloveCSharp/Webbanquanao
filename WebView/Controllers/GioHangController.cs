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

        // Hiển thị giỏ hàng
        public async Task<IActionResult> Index()
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            var gioHangItems = await _context.GioHangs
                .Include(g => g.ChiTietSanPham)
                .ThenInclude(sp => sp.SanPham) // Include thông tin sản phẩm gốc
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
        public IActionResult ProceedToCheckout()
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            var gioHangItems = _context.GioHangs
                .Include(g => g.ChiTietSanPham)
                .ThenInclude(sp => sp.SanPham)
                .Where(g => g.Id_KhachHang == khachHangId && g.TrangThai)
                .ToList();

            if (!gioHangItems.Any())
            {
                return RedirectToAction("Index", "GioHang");
            }

            // Trả về trang điền thông tin liên hệ và phương thức thanh toán
            return View("Checkout", gioHangItems);
        }
        [HttpPost]
        [HttpPost]
        public IActionResult CompleteOrder(string FullName, string Address, string Phone, string PaymentMethod)
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            var gioHangItems = _context.GioHangs
                .Include(g => g.ChiTietSanPham)
                .ThenInclude(sp => sp.SanPham)
                .Where(g => g.Id_KhachHang == khachHangId && g.TrangThai)
                .ToList();

            if (!gioHangItems.Any())
            {
                return RedirectToAction("Index", "GioHang");
            }

            // Tính tổng tiền
            decimal tongTien = gioHangItems.Sum(item => item.SoLuong * decimal.Parse(item.ChiTietSanPham.SanPham.Gia));

            // Tạo đối tượng hóa đơn mới và lưu vào cơ sở dữ liệu
            var hoaDon = new HoaDon
            {
                Id_KhachHang = khachHangId.Value,
                TongTien = tongTien.ToString("#,##0 VNĐ"),
                NgayTao = DateTime.Now,
                TrangThai = 1, // Đơn hàng mới đặt, trạng thái là 1 (Chờ xử lý)
                Id_NhanVien = 1,
            };

            _context.HoaDons.Add(hoaDon);
            _context.SaveChanges();

            // Sau khi lưu hóa đơn, lưu chi tiết hóa đơn
            foreach (var item in gioHangItems)
            {
                var chiTietHoaDon = new ChiTietHoaDon
                {
                    Id_HoaDon = hoaDon.Id,
                    Id_ChiTietSanPham = item.Id_ChiTietSanPham,
                    SoLuong = item.SoLuong,
                    Gia = item.ChiTietSanPham.SanPham.Gia
                };

                _context.ChiTietHoaDons.Add(chiTietHoaDon);
            }

            _context.SaveChanges();

            // Xóa giỏ hàng sau khi đặt hàng thành công
            foreach (var item in gioHangItems)
            {
                _context.GioHangs.Remove(item);
            }

            _context.SaveChanges();

            return RedirectToAction("XacNhanThanhToan");
        }

        public IActionResult XacNhanThanhToan()
        {
            return View();
        }


    }
}
