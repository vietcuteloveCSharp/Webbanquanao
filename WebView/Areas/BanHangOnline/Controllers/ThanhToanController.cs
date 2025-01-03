using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebView.Areas.BanHangOnline.HoangDTO.Param;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;
using WebView.Models.Vnpay;
using WebView.Repository;
using WebView.Services.Vnpay;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class ThanhToanController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IVnPayService _vnPayService;
        public ThanhToanController(WebBanQuanAoDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        public async Task<IActionResult> Index()
        {
            var resp = new ThanhToanResp();
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            // list giỏ hàng của khách hàng
            var lstGioHang = await _context.GioHangs.Where(x => x.Id_KhachHang == tk.Id && x.SoLuong > 0).Include(x => x.ChiTietSanPham).ToListAsync();

            if (lstGioHang == null || lstGioHang.Count <= 0)
            {
                ViewData["SpGioHang"] = new ThanhToanResp();
                return View();
            }
            // list id san pham chi tiet trong giỏ hàng
            var lstIdSpCT = lstGioHang.Select(x => x.Id_ChiTietSanPham).Distinct().ToList();
            // list san pham theo id spct có trong giỏ hàng
            var lstSp = await _context.ChiTietSanPhams.Where(x => lstIdSpCT.Contains(x.Id)).Include(x => x.SanPham).Include(x => x.MauSac).Include(x => x.KichThuoc).Select(x => x.SanPham).Distinct().ToListAsync();
            var lstIdSp = lstSp.Select(x => x.Id).Distinct().ToList();
            // list hinh anh theo sp
            var lstHinhAnh = _context.HinhAnhs.Where(x => lstIdSp.Contains((int)x.Id_SanPham)).ToList();
            // list sp có hình ảnh kèm theo
            var lstSpVoiHinhAnh = lstSp.Select(x => new
            {
                IdSP = x?.Id,
                ListHinHAnh = lstHinhAnh.Where(a => a.Id_SanPham == x.Id).Take(1).Select(a => new HinhAnhResp
                {
                    Id = a.Id,
                    Id_SanPham = x.Id,
                    ImageData = a.ImageData,
                    Url = a.Url
                }).ToList()
            }).ToList();
            // Lấy list Mau sac va kich thuoc dựa trên sp sản phẩm chi tiết
            var lstSpCTTheoIdSp = await _context.ChiTietSanPhams.Where(x => lstIdSp.Contains(x.Id_SanPham)).Include(x => x.MauSac).Include(x => x.KichThuoc).ToListAsync();
            var lstMauSac = lstSpCTTheoIdSp.Select(x => new
            {
                Id = x.MauSac.Id,
                MaHex = x.MauSac.MaHex,
                Ten = x.MauSac.Ten,
                IdSp = x.Id_SanPham
            }).Distinct().ToList(); ;
            var lstKichThuoc = lstSpCTTheoIdSp.Select(x => new
            {
                Id = x.KichThuoc.Id,
                Ten = x.KichThuoc.Ten,
                IdSp = x.Id_SanPham,
                IdMs = x.Id_MauSac
            }).Distinct().ToList();
            // Tổng hợp lại toàn bộ dựa trên list giỏ hàng

            resp = new ThanhToanResp
            {
                GioHang = lstGioHang.Select(x => new GioHangResp
                {
                    Id = x?.Id,
                    SoLuong = x?.SoLuong,
                    IdChiTietSanPham = x?.Id_ChiTietSanPham,
                    IdKichThuoc = x?.ChiTietSanPham?.Id_KichThuoc,
                    IdMauSac = x?.ChiTietSanPham?.Id_MauSac,
                    KichThuocResps = lstKichThuoc.Where(a => a.IdSp == x.ChiTietSanPham.Id_SanPham && a.Id == x.ChiTietSanPham?.Id_KichThuoc).Select(a => new KichThuocResp
                    {
                        Id = a?.Id,
                        Ten = a?.Ten,
                        Id_MauSac = x?.ChiTietSanPham?.Id_MauSac
                    }).Distinct().ToList(),
                    MauSacResps = lstMauSac.Where(a => a.IdSp == x.ChiTietSanPham.Id_SanPham && a.Id == x.ChiTietSanPham?.Id_MauSac).Select(x => new MauSacResp
                    {
                        Id = x.Id,
                        MaHex = x.MaHex,
                        Ten = x.Ten
                    }).Distinct().ToList(),
                    SanPham = lstSp.Where(a => a.Id == x.ChiTietSanPham.Id_SanPham).Select(a => new SanPhamResp
                    {
                        Id = a.Id,
                        GiaBan = a.Gia,
                        GiaBanDau = 0,
                        MoTa = a.MoTa,
                        SoLuong = a.SoLuong,
                        Ten = a.Ten,
                        ListHinHAnh = lstSpVoiHinhAnh.FirstOrDefault(b => b.IdSP == a.Id).ListHinHAnh,
                    }).FirstOrDefault()
                }).ToList(),
                KhachHangModel = new KhachHangResp
                {
                    Id = tk?.Id,
                    Avatar = tk?.Avatar,
                    Email = tk?.Email,
                    Sdt = tk?.Sdt,
                    Ten = tk?.Ten,
                    TrangThai = tk.TrangThai,
                }
            };
            ViewData["SpThanhToan"] = resp;
            HttpContext.Session.Remove("SpThanhToan");
            HttpContext.Session.SetString("SpThanhToan", JsonConvert.SerializeObject(resp));
            return View();
        }
        // api: nhận thông tin thanh toán
        //  request: 
        [HttpPost]
        public async Task<IActionResult> PostThanhToanDonHang(ThanhToanParam req)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        [HttpGet]
        public IActionResult ThanhToanThanhCong()
        {
            return Json("Thanh toan thanh cong");
        }
        [HttpGet]
        public async Task<IActionResult> GetThanhToan()
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            // trả về thông tin cần thanh toán
            return View();
        }
    }
}
