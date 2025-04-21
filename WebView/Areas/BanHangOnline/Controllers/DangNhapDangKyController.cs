using DAL.Context;
using DAL.Entities;
using Enum.EnumVVA;
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
using WebView.Areas.BanHangOnline.HoangDTO.Resp;
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

        [HttpGet]
        public async Task<IActionResult> DonHang()
        {
            ViewData["type"] = "donhang";
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            // lấy id phương thức thanh toán cod và vnpay
            //var lstIdThanhToan = await _context.PhuongThucThanhToans
            //    .Where(x => x.Ten.ToLower().Equals("cod") || x.Ten.ToLower().Equals("vnpay"))
            //    .Select(x => x.Id).ToListAsync();
            //if (lstIdThanhToan == null || lstIdThanhToan.Count == 0)
            //{
            //    lstIdThanhToan.Add(0);
            //}

            // lấy danh sách hóa đơn
            var lstHoaDon = await _context.HoaDons.Where(x => x.Id_KhachHang == tk.Id)
                .Include(x => x.ChiTietHoaDons)
                //.Where(x => x.ThanhToanHoaDons.Any(a => lstIdThanhToan.Contains(a.Id_PhuongThucThanhToan)))
                .ToListAsync();

            var resp = lstHoaDon.Select(x => new HoaDonKhachHangResp
            {
                Id = x.Id,
                TongTien = Math.Round(x.TongTien),
                NgayMua = x.NgayTao,
                SoLuongSp = x.ChiTietHoaDons.Count,
                TrangThai = (int)x.TrangThai

            }).OrderByDescending(x => x.NgayMua).ToList();
            ViewData["lstHoaDon"] = resp;
            return View("TaiKhoan", tk);
        }

        [HttpGet]
        public async Task<IActionResult> DonHangChiTiet(int id)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            // dựa vào id hóa đơn -> lấy được hóa đơn -> lấy được ds chi tiết sản phẩm + phương thức thanh toán hóa đơn
            var hoadon = await _context.HoaDons.Include(x => x.ChiTietHoaDons).ThenInclude(x => x.ChiTietSanPham.SanPham)
                                               .Include(x => x.ThanhToanHoaDons).ThenInclude(x => x.PhuongThucThanhToan)
                                               .FirstOrDefaultAsync(x => x.Id == id);
            if (hoadon == null)
            {
                return View();
            }
            string trangthai = "";
            switch (hoadon.TrangThai)
            {
                case ETrangThaiHD.None:
                    trangthai = "";
                    break;
                case ETrangThaiHD.HoanThanhDon:
                    trangthai = "Hoàn thành đơn";
                    break;
                case ETrangThaiHD.ChoXacNhan:
                    trangthai = "Chờ xác nhận";
                    break;
                case ETrangThaiHD.ChoThanhToan:
                    trangthai = "Chờ thanh toán";
                    break;
                case ETrangThaiHD.DaXacNhan:
                    trangthai = "Đã xác nhận";
                    break;
                case ETrangThaiHD.DangVanChuyen:
                    trangthai = "Đang vận chuyển";
                    break;
                case ETrangThaiHD.HuyDon:
                    trangthai = "Hủy đơn";
                    break;
            }
            var banHangTaiQuay = _context.PhuongThucThanhToans.FirstOrDefault(x => x.Ten.ToLower().Equals("bantaiquay"))?.Id ?? 0;

            string phuongThucTT = "Thanh toán tại quầy";
            string hinhThucGiaoHang = "Giao hàng tận nơi";
            switch (hoadon.ThanhToanHoaDons.First().PhuongThucThanhToan.Ten.ToLower())
            {
                case "vnpay":
                    phuongThucTT = "Thanh toán qua Vnpay";
                    break;
                case "cod":
                    phuongThucTT = "Thanh toán khi nhận hàng";
                    break;
                case "bantaiquay":
                    phuongThucTT = "Thanh toán tại quầy";
                    hinhThucGiaoHang = "Tại quầy";
                    break;
                default:
                    phuongThucTT = "Phương thức thanh toán khác";
                    break;
            }
            var resp = new HoaDonChiTietResp()
            {
                DiaChiGiaoHang = hoadon.DiaChiGiaoHang,
                NgayMua = hoadon.NgayTao,
                PhiVanChuyen = Math.Round(hoadon.PhiVanChuyen),
                TongTien = Math.Round(hoadon.TongTien),
                TrangThai = trangthai,
                SanPhamResp = hoadon.ChiTietHoaDons.Select(x => new HoaDonSanPhamChiTietResp
                {
                    Gia = x.Gia,
                    SoLuong = x.SoLuong,
                    MoTa = x.ChiTietSanPham.SanPham.MoTa,
                    Ten = x.ChiTietSanPham.SanPham.Ten,
                    KichThuoc = _context.KichThuocs.Where(a => a.Id == x.ChiTietSanPham.Id_KichThuoc).Select(a => new KichThuocResp()
                    {
                        Id = a.Id,
                        Ten = a.Ten
                    }).FirstOrDefault(),
                    MauSac = _context.MauSacs.Where(a => a.Id == x.ChiTietSanPham.Id_MauSac).Select(a => new MauSacResp()
                    {
                        Id = a.Id,
                        Ten = a.Ten,
                        MaHex = a.MaHex
                    }).FirstOrDefault(),
                    HinhAnh = _context.HinhAnhs.FirstOrDefault(a => a.Id_SanPham == x.ChiTietSanPham.Id_SanPham).Url
                }).ToList(),
                HinhThucMuaHang = hoadon.ThanhToanHoaDons.First().Id_PhuongThucThanhToan == banHangTaiQuay ? "Offline" : "Online",
                PhuongThucThanhToan = phuongThucTT,
                HinhThucGiaoHang = hinhThucGiaoHang,
                SDT = tk.Sdt,
                TenKhachHang = tk.Ten
            };

            ViewData["HoaDonChiTiet"] = resp;
            return View("HoaDonChiTiet");
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

        [HttpPost]
        public IActionResult DoiMatKhau(DoiMatKhauParam request)
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

            kh.MatKhau = request.Password;
            _context.SaveChanges();
            ViewData["message"] = "Đổi mật khẩu thành công";
            return View("TaiKhoan", kh);
        }

        public async Task<IActionResult> DiaChiKhachHang()
        {
            ViewData["type"] = "diachicuatoi";
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
            var resp = await _context.DiaChiKhachHangs.AsNoTracking().Where(x => x.IdKhachHang == kh.Id).Where(x => x.TrangThai)?
                                .Select(x => new DiaChiKhachHangResp
                                {
                                    Id = x.Id,
                                    ChiTietDiaChi = x.ChiTietDiaChi,
                                    IdPhuong = x.IdPhuong,
                                    IdQuan = x.IdQuan,
                                    IdTinh = x.IdTinh,
                                    IsDefault = x.IsDefault,
                                    TenPhuong = x.TenPhuong,
                                    TenQuan = x.TenQuan,
                                    TenTinh = x.TenTinh
                                }).OrderByDescending(x => x.IsDefault).ThenBy(x => x.Id).ToListAsync();
            ViewData["diachigiaohang"] = resp;
            return View("TaiKhoan", kh);
        }

        [HttpPost]
        public async Task<IActionResult> DiaChiKhachHang([FromBody] DiaChiKhachHangParam request)
        {
            ViewData["type"] = "diachicuatoi";
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            if (request == null || string.IsNullOrEmpty(request.IdTinh) || string.IsNullOrEmpty(request.IdPhuong) || string.IsNullOrEmpty(request.IdQuan))
            {
                ViewData["mess"] = "Thêm thất bại địa chỉ";
                ViewData["status"] = false;
                return Json(new { status = true, mess = "Thêm thất bại địa chỉ" });
            }
            if (_context.DiaChiKhachHangs.Count(x => x.IdKhachHang == tk.Id) == 0)
            {
                request.IsDefault = true;
            }
            else
            {
                if (request.IsDefault)
                {
                    var diachibd = _context.DiaChiKhachHangs.Where(x => x.IdKhachHang == tk.Id && x.IsDefault == true && x.TrangThai).ToList();
                    foreach (var item in diachibd)
                    {
                        item.IsDefault = false;
                    }
                    _context.SaveChanges();
                }
            }
            await _context.DiaChiKhachHangs.AddAsync(new DAL.Entities.DiaChiKhachHang
            {
                ChiTietDiaChi = request.ChiTietDiaChi,
                IdKhachHang = tk.Id,
                IdPhuong = request.IdPhuong,
                IdQuan = request.IdQuan,
                IdTinh = request.IdTinh,
                IsDefault = request.IsDefault,
                TenPhuong = request.TenPhuong,
                TenQuan = request.TenQuan,
                TenTinh = request.TenTinh,
                TrangThai = true
            });
            _context.SaveChanges();
            ViewData["mess"] = "Thêm thành công địa chỉ";
            ViewData["status"] = true;

            return Json(new { status = true, mess = "Thêm thành công địa chỉ" });
        }
        [HttpPost]
        public async Task<IActionResult> ChinhSuaDiaChi([FromBody] ChinhSuaDiaChiKhachHangParam request)
        {
            ViewData["type"] = "diachicuatoi";
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            if (request == null || string.IsNullOrEmpty(request.Id))
            {
                ViewData["mess"] = "Chỉnh sửa thất bại địa chỉ";
                ViewData["status"] = false;
                return Json(new { status = false, mess = "Chỉnh sửa thất bại địa chỉ" });
            }
            var diachi = await _context.DiaChiKhachHangs.FirstOrDefaultAsync(x => x.Id == int.Parse(request.Id) && x.TrangThai);
            if (diachi == null)
            {
                ViewData["mess"] = "Chỉnh sửa thất bại địa chỉ";
                ViewData["status"] = false;
                return Json(new { status = false, mess = "Chỉnh sửa thất bại địa chỉ" });

            }
            if (diachi.IsDefault == false && request.IsDefault)
            {
                var diachimacdinh = _context.DiaChiKhachHangs.FirstOrDefault(x => x.IsDefault && x.TrangThai);
                if (diachimacdinh != null)
                {
                    diachimacdinh.IsDefault = false;
                    _context.SaveChanges();
                }
            }
            diachi.ChiTietDiaChi = string.IsNullOrEmpty(request.ChiTietDiaChi) ? diachi.ChiTietDiaChi : request.ChiTietDiaChi.Trim();
            diachi.IdPhuong = string.IsNullOrEmpty(request.IdPhuong) ? diachi.IdPhuong : request.IdPhuong.Trim();
            diachi.IdQuan = string.IsNullOrEmpty(request.IdQuan) ? diachi.IdQuan : request.IdQuan.Trim();
            diachi.IdTinh = string.IsNullOrEmpty(request.IdTinh) ? diachi.IdTinh : request.IdTinh.Trim();
            diachi.IsDefault = request.IsDefault;
            diachi.TenPhuong = string.IsNullOrEmpty(request.TenPhuong) ? diachi.TenPhuong : request.TenPhuong.Trim();
            diachi.TenQuan = string.IsNullOrEmpty(request.TenQuan) ? diachi.TenQuan : request.TenQuan.Trim();
            diachi.TenTinh = string.IsNullOrEmpty(request.TenTinh) ? diachi.TenTinh : request.TenTinh.Trim();
            _context.SaveChanges();

            ViewData["mess"] = "Chỉnh sửa thành công địa chỉ";
            ViewData["status"] = true;

            return Json(new { status = true, mess = "Chỉnh sửa thành công địa chỉ" });
        }
        [HttpDelete]
        public async Task<IActionResult> XoaDiaChi(int id)
        {
            ViewData["type"] = "diachicuatoi";
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return View("ChuaDangNhap");
            }
            if (_context.DiaChiKhachHangs.Count() <= 1)
            {
                return Json(new { status = false, mess = "Xóa dịa chỉ thất bại" });
            }
            var diachi = await _context.DiaChiKhachHangs.FirstOrDefaultAsync(x => x.Id == id && x.TrangThai);
            if (diachi == null)
            {
                return Json(new { status = false, mess = "Xóa dịa chỉ thất bại" });
            }
            if (_context.DiaChiKhachHangs.Count(x => x.TrangThai) <= 1)
            {
                return Json(new { status = false, mess = "Xóa dịa chỉ thất bại" });
            }
            diachi.TrangThai = false;
            if (diachi.IsDefault)
            {
                var diachimacdinhmoi = _context.DiaChiKhachHangs.FirstOrDefault(x => x.TrangThai && diachi.Id != x.Id);
                diachimacdinhmoi.IsDefault = true;
            }
            _context.SaveChanges();

            return Json(new { status = true, mess = "Xóa địa chỉ thành công" });
        }
        public IActionResult DangXuat()
        {
            HttpContext.Session.Remove("TaiKhoan");
            HttpContext.Session.Clear();
            return View("DangXuat");
        }
    }
}
