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
using System.Text.Json;
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
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("Index");
            }
            return RedirectToAction("TaiKhoan");
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
                ViewData["State"] = JsonSerializer.Serialize("Đăng nhập thất bại.");
                ViewData["IsError"] = true;
                return View("Index");
            }
            var khachHang = await _context.KhachHangs
               .FirstOrDefaultAsync(kh => kh.TaiKhoan == param.TaiKhoanDN && kh.MatKhau == param.PasswordDN);

            if (khachHang != null)
            {
                // Lưu Id của KhachHang vào session
                HttpContext.Session.SetObjectAsJson("TaiKhoan", khachHang);
                ViewData["State"] = JsonSerializer.Serialize("Đăng nhập thành công.");
                ViewData["IsError"] = false;
                return Redirect("/BanHangOnline/TrangChu/Index");
            }
            ViewData["State"] = JsonSerializer.Serialize("Đăng nhập thất bại.");
            ViewData["IsError"] = true;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DangKy(DangKyParam param)
        {
            if (string.IsNullOrEmpty(param.TaikhoanDK) || string.IsNullOrEmpty(param.NameFullDK) || param.NgaySinhDK == null || string.IsNullOrEmpty(param.RegisterPhoneDK) || string.IsNullOrEmpty(param.EmailDK) || string.IsNullOrEmpty(param.RegisterPasswordDK) || string.IsNullOrEmpty(param.ConfirmPasswordDK))
            {
                ViewData["State"] = JsonSerializer.Serialize("Đăng ký thất bại.");
                ViewData["IsError"] = true;
                return View("Index");
            }
            if (_context.KhachHangs.Any(x => x.TaiKhoan.Equals(param.TaikhoanDK)))
            {
                ViewData["State"] = JsonSerializer.Serialize("Đăng ký thất bại.");
                ViewData["IsError"] = true;
                return View("Index");
            }
            if (!param.RegisterPasswordDK.Equals(param.ConfirmPasswordDK))
            {
                ViewData["State"] = JsonSerializer.Serialize("Đăng ký thất bại.");
                ViewData["IsError"] = true;
                return View("Index");
            }
            _context.KhachHangs.Add(new KhachHang
            {
                TaiKhoan = param.TaikhoanDK.Trim(),
                Ten = param.NameFullDK.Trim(),
                NgaySinh = param.NgaySinhDK,
                Email = param.EmailDK.Trim(),
                Sdt = param.RegisterPhoneDK.Trim(),
                MatKhau = param.RegisterPasswordDK.Trim()
            });
            _context.SaveChanges();
            ViewData["State"] = JsonSerializer.Serialize("Đăng ký thành công.");
            ViewData["IsError"] = false;
            return View("Index");
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
        [HttpGet]
        public IActionResult ChuaDangNhap()
        {
            return View();
        }
        private string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public IActionResult TaiKhoan()
        {
            ViewData["type"] = "taikhoan";
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            var kh = _context.KhachHangs.FirstOrDefault(x => x.Id == tk.Id);
            if (kh == null)
            {
                return View("ChuaDangNhap");

            }

            return View("TaiKhoan", kh);
        }
        [HttpPost]
        public IActionResult PostTaiKhoan(TaiKhoanModel req)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            var kh = _context.KhachHangs.FirstOrDefault(x => x.Id == tk.Id);
            if (kh == null)
            {
                return View("ChuaDangNhap");

            }
            kh.Ten = req.ten.Trim();
            _context.SaveChanges();
            ViewData["type"] = "taikhoan";
            return RedirectToAction("TaiKhoan");
        }
        public IActionResult DoiMatKhau()
        {
            ViewData["type"] = "doimatkhau";
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            var kh = _context.KhachHangs.FirstOrDefault(x => x.Id == tk.Id);
            if (kh == null)
            {
                return View("ChuaDangNhap");

            }
            return View("TaiKhoan", kh);
        }
        public IActionResult DangXuat()
        {
            HttpContext.Session.Remove("TaiKhoan");
            HttpContext.Session.Clear();
            return View("DangXuat");
        }
    }
}
