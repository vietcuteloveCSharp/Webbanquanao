using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebView.Areas.BanHangOnline.HoangDTO;
using WebView.Areas.BanHangOnline.HoangDTO.Param;
using WebView.Repository;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class DangNhapDangKyController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        private IOptions<JwtSection> _options;
        // Sử dụng Dependency Injection để lấy HttpClient
        public DangNhapDangKyController(WebBanQuanAoDbContext context, IOptions<JwtSection> options)
        {
            _context = context;
            _options = options;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult XacNhanOtp()
        {
            return View("XacNhanOtp");
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(DangNhapParam param)
        {
            if (string.IsNullOrEmpty(param.TaiKhoanDN) || string.IsNullOrEmpty(param.PasswordDN))
            {
                ModelState.AddModelError(string.Empty, "Xin nhập tài khoản và mật khẩu");
                return View();
            }
            var khachHang = await _context.KhachHangs
               .FirstOrDefaultAsync(kh => kh.TaiKhoan == param.TaiKhoanDN && kh.MatKhau == param.PasswordDN);

            if (khachHang != null)
            {
                // Lưu Id của KhachHang vào session
                HttpContext.Session.SetObjectAsJson("TaiKhoan", khachHang);

                // Lấy giỏ hàng từ cookie
                //var cookieCart = Request.Cookies.GetObjectFromJson<List<GioHang>>("GioHang");
                //if (cookieCart != null)
                //{
                //    foreach (var item in cookieCart)
                //    {
                //        var existingItem = await _context.GioHangs
                //            .FirstOrDefaultAsync(g => g.Id_ChiTietSanPham == item.Id_ChiTietSanPham && g.Id_KhachHang == khachHang.Id);

                //        if (existingItem != null)
                //        {
                //            // Cập nhật số lượng nếu sản phẩm đã có trong giỏ hàng
                //            existingItem.SoLuong += item.SoLuong;
                //            _context.GioHangs.Update(existingItem);
                //        }
                //        else
                //        {
                //            // Thêm mới sản phẩm vào giỏ hàng
                //            var newGioHangItem = new GioHang
                //            {
                //                Id_KhachHang = khachHang.Id,
                //                Id_ChiTietSanPham = item.Id_ChiTietSanPham,
                //                SoLuong = item.SoLuong,
                //                TrangThai = true
                //            };
                //            _context.GioHangs.Add(newGioHangItem);
                //        }
                //    }

                //    await _context.SaveChangesAsync();

                //    // Xóa cookie sau khi chuyển giỏ hàng thành công
                //    Response.Cookies.Delete("GioHang");
                //}
                return Redirect("/BanHangOnline/TrangChu/Index");
            }

            ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu.");
            return View();
        }

        private string GenerationToken(KhachHang taiKhoan, string role)
        {
            // ma hoa key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key!));
            var crenditals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // tao list claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,taiKhoan.Id.ToString()),
                new Claim(ClaimTypes.Name,taiKhoan.Ten.ToString()),
                new Claim(ClaimTypes.Email,taiKhoan.Email.ToString()),
                new Claim(ClaimTypes.Role,role.ToString()),
            };

            var token = new JwtSecurityToken(
                 issuer: _options.Value.Issuer!,
                    audience: _options.Value.Audience!,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: crenditals
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    }
}
