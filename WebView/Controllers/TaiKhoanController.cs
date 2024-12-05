using DAL.Context;
using DAL.Entities;
using DAL.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public TaiKhoanController(WebBanQuanAoDbContext dbcontext)
        {
            _context = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string taiKhoan, string matKhau)
        {
            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(kh => kh.TaiKhoan == taiKhoan && kh.MatKhau == matKhau);

            if (khachHang != null)
            {
                // Lưu Id của KhachHang vào session
                HttpContext.Session.SetInt32("KhachHangId", khachHang.Id);
                HttpContext.Session.SetString("TaiKhoan", khachHang.TaiKhoan);

                // Lấy giỏ hàng từ cookie
                var cookieCart = Request.Cookies.GetObjectFromJson<List<GioHang>>("GioHang");
                if (cookieCart != null)
                {
                    foreach (var item in cookieCart)
                    {
                        var existingItem = await _context.GioHangs
                            .FirstOrDefaultAsync(g => g.Id_ChiTietSanPham == item.Id_ChiTietSanPham && g.Id_KhachHang == khachHang.Id);

                        if (existingItem != null)
                        {
                            // Cập nhật số lượng nếu sản phẩm đã có trong giỏ hàng
                            existingItem.SoLuong += item.SoLuong;
                            _context.GioHangs.Update(existingItem);
                        }
                        else
                        {
                            // Thêm mới sản phẩm vào giỏ hàng
                            var newGioHangItem = new GioHang
                            {
                                Id_KhachHang = khachHang.Id,
                                Id_ChiTietSanPham = item.Id_ChiTietSanPham,
                                SoLuong = item.SoLuong,
                                TrangThai = true
                            };
                            _context.GioHangs.Add(newGioHangItem);
                        }
                    }

                    await _context.SaveChangesAsync();

                    // Xóa cookie sau khi chuyển giỏ hàng thành công
                    Response.Cookies.Delete("GioHang");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu.");
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Handle the creation of a new account
        [HttpPost]
        [ValidateAntiForgeryToken] // To protect against CSRF attacks
        public async Task<IActionResult> Create([Bind("TaiKhoan,MatKhau,Email")] KhachHang model)
        {
            if (ModelState.IsValid)
            {
                // Set default values for other fields
                model.NgaySinh = null; // Optional
                model.Ten = string.Empty; // Optional
                model.Sdt = string.Empty; // Optional
                model.TrangThai = true; // Default active status

                // Initialize collections if needed
                model.GioHangs = new List<GioHang>(); // Initialize as empty list
                model.HoaDons = new List<HoaDon>(); // Initialize as empty list

                // Add the new customer to the database
                _context.KhachHangs.Add(model);
                await _context.SaveChangesAsync();

                // Redirect to a page after successful registration
                return RedirectToAction("Login", "TaiKhoan");
            }

            return View(model); // Return to the view if the model state is invalid
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa tất cả session
            return RedirectToAction("Login", "TaiKhoan"); // Chuyển hướng đến trang đăng nhập
        }
        // Method to show user profile
        public async Task<IActionResult> Profile()
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login");
            }

            // Lấy thông tin khách hàng cùng với giỏ hàng và hóa đơn
            var khachHang = await _context.KhachHangs
                .Include(kh => kh.HoaDons) // Bao gồm thông tin hóa đơn
                .Include(kh => kh.GioHangs) // Bao gồm thông tin giỏ hàng
                .ThenInclude(g => g.ChiTietSanPham) // Bao gồm chi tiết sản phẩm trong giỏ hàng
                .FirstOrDefaultAsync(kh => kh.Id == khachHangId);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang); // Trả về View 'Profile' với thông tin khách hàng
        }



        // Method to edit profile
        public async Task<IActionResult> EditProfile()
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login");
            }

            var khachHang = await _context.KhachHangs.FindAsync(khachHangId);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy Id khách hàng từ session
                    var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

                    if (khachHangId == null)
                    {
                        return RedirectToAction("Login");
                    }

                    // Tìm khách hàng trong cơ sở dữ liệu
                    var khachHang = await _context.KhachHangs
                        .FirstOrDefaultAsync(kh => kh.Id == khachHangId);

                    if (khachHang == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật các thông tin từ model được gửi lên
                    khachHang.Email = model.Email;
                    khachHang.NgaySinh = model.NgaySinh;
                    khachHang.Ten = model.Ten;
                    khachHang.Sdt = model.Sdt;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.KhachHangs.Update(khachHang);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Profile");
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Kiểm tra xem khách hàng còn tồn tại hay không
                    if (!_context.KhachHangs.Any(kh => kh.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Trả về lại trang chỉnh sửa nếu model không hợp lệ
            return View(model);
        }

    }
}
