﻿using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebView.Areas.BanHangOnline.HoangDTO.Param;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;
using WebView.Repository;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class ChiTietSanPhamController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public ChiTietSanPhamController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSanPhamChiTiet(int id = -1)
        {
            var resp = new ChiTietSanPhamResp();
            if (id <= 0)
            {
                return View("SanPhamKhongTonTai");
            }
            var sp = await _context.SanPhams.Include(x => x.ThuongHieu).FirstOrDefaultAsync(x => x.Id == id);
            if (sp == null)
            {
                return View("SanPhamKhongTonTai");
            }
            var lstHinHAnh = await _context.HinhAnhs.Where(x => x.Id_SanPham == sp.Id).ToListAsync();
            resp.SanPham = new SanPhamResp
            {
                Id = sp?.Id,
                GiaBan = sp.Gia,
                GiaBanDau = 0,
                Id_DanhMuc = sp?.Id_DanhMuc,
                MoTa = sp?.MoTa,
                SoLuong = sp?.SoLuong ?? 0,
                Ten = sp?.Ten,
                ListHinHAnh = lstHinHAnh.Select(x => new HinhAnhResp
                {
                    Id = x?.Id,
                    Id_SanPham = x?.Id_SanPham,
                    ImageData = x?.ImageData,
                    Url = x?.Url,
                }).ToList(),
            };
            // list sp chi tiet 
            var lstSpCT = await _context.ChiTietSanPhams.Include(x => x.MauSac).Include(x => x.KichThuoc).Where(x => x.Id_SanPham == sp.Id)?.ToListAsync();
            if (lstSpCT != null && lstSpCT.Count > 0)
            {
                // list mau sac
                var lstMauSac = lstSpCT?.Select(x => x.MauSac)?.Distinct().OrderBy(x => x.Id).ToList();
                // list kich thuoc
                var lstKichThuoc = lstSpCT?.Select(x => new KichThuocResp
                {
                    Id = x?.KichThuoc?.Id,
                    Ten = x?.KichThuoc?.Ten,
                    Id_MauSac = x?.MauSac?.Id
                })?.Distinct().OrderBy(x => x.Id_MauSac).ThenBy(x => x.Ten).ToList();


                resp.MauSacResps = lstMauSac?.Select(x => new MauSacResp
                {
                    Id = x?.Id,
                    MaHex = x?.MaHex,
                    Ten = x?.Ten
                })?.Distinct().ToList();
                resp.KichThuocResps = lstKichThuoc;
            }
            ViewData["sanphamchitiet"] = resp;
            HttpContext.Session.SetString("ChiTietSanPham", JsonConvert.SerializeObject(resp));
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PostSanPhamVaoGioHang([FromBody] AddSanPhamGioHang param)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            var a = new
            {
                param = param,
                tk = tk,
            };
            return Json(a);
        }
        public string GetSessionSanPhamChiTiet(int id = -1)
        {
            if (id > 0)
            {
                // Lấy chuỗi từ Session
                string userName = HttpContext.Session.GetString("ChiTietSanPham");
                var resp = JsonConvert.DeserializeObject<ChiTietSanPhamResp>(userName);
                if (resp.KichThuocResps.Any(x => x.Id_MauSac == id))
                {
                    Content($"{JsonConvert.DeserializeObject<ChiTietSanPhamResp>(userName)}");
                    return JsonConvert.SerializeObject(resp.KichThuocResps.Where(x => x.Id_MauSac == id).ToList());
                }
            }
            return null;
        }
        public IActionResult ClearSession()
        {
            // Xóa toàn bộ Session
            HttpContext.Session.Clear();
            return Content("Session đã được xóa.");
        }
    }
}
