using DAL.Context;
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
            var sp = await _context.SanPhams.Include(x => x.ThuongHieu).Where(x => x.SoLuong > 0).FirstOrDefaultAsync(x => x.Id == id);
            if (sp == null)
            {
                return View("SanPhamKhongTonTai");
            }
            // Tìm khuyến mại cho sản phẩm
            var timenow = DateTime.Now;
            var khuyenMai = _context.ChiTietKhuyenMais.Where(x => x.Id_DanhMuc == sp.Id_DanhMuc)
                .Include(x => x.KhuyenMai)
                .Where(x => x.KhuyenMai.TrangThai == 1)
                .Where(x => x.KhuyenMai.NgayBatDau <= timenow && timenow < x.KhuyenMai.NgayKetThuc)
                .FirstOrDefault();
            var giaBan = khuyenMai != null && sp.Gia >= khuyenMai?.KhuyenMai?.DieuKienGiamGia ? Math.Round(sp.Gia - (sp.Gia * khuyenMai.KhuyenMai.GiaTriGiam / 100)) : Math.Round(sp.Gia);
            var lstHinHAnh = await _context.HinhAnhs.Where(x => x.Id_SanPham == sp.Id).ToListAsync();
            resp.SanPham = new SanPhamResp
            {
                Id = sp?.Id,
                GiaBan = giaBan,
                GiaBanDau = Math.Round(sp.Gia),
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
            var lstSpCT = await _context.ChiTietSanPhams.Include(x => x.MauSac).Include(x => x.KichThuoc).Where(x => x.SoLuong > 0).Where(x => x.Id_SanPham == sp.Id)?.ToListAsync();
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
                })?.DistinctBy(x => x.Id).OrderBy(x => x.Id_MauSac).ThenBy(x => x.Ten).ToList();


                resp.MauSacResps = lstMauSac?.Select(x => new MauSacResp
                {
                    Id = x?.Id,
                    MaHex = x?.MaHex,
                    Ten = x?.Ten
                })?.DistinctBy(x => x.Id).ToList();
                resp.KichThuocResps = lstKichThuoc;
            }
            ViewData["sanphamchitiet"] = resp;
            // Xóa Session cũ có tên "ChiTietSanPham"
            HttpContext.Session.Remove("ChiTietSanPham");
            HttpContext.Session.SetString("ChiTietSanPham", JsonConvert.SerializeObject(resp));
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PostSanPhamVaoGioHang([FromBody] AddSanPhamGioHang param)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            var spReq = new
            {
                idSP = int.Parse(param.IdSanPham),
                idMs = int.Parse(param.IdMauSac),
                idKt = int.Parse(param.IdKichThuoc),
                soLuongSp = int.Parse(param.SoLuong)
            };
            // Kiểm tra kh đã thêm sản phẩm này vào giỏ hàng hay chưa -> chưa : thêm mới sản phẩm vào giỏ hàng với sp =1 || rồi : tăng số số lượng thêm 1 trong giỏ hàng của kh
            // toàn bộ có trạng thái là 1 == hiển thị
            // tìm sản phẩm chi tiết
            var spct = await _context.ChiTietSanPhams.Where(x => x.SoLuong > 0 && spReq.soLuongSp > 0 && x.SoLuong >= spReq.soLuongSp)
                .FirstOrDefaultAsync(x => x.Id_SanPham == spReq.idSP && x.Id_MauSac == spReq.idMs && x.Id_KichThuoc == spReq.idKt);
            if (spct == null)
            {
                return Json(new { status = 404, success = false, message = "Không tìm thấy sản phẩm" });
            }
            string mess = "";
            // Kiểm tra giỏ hàng khách hàng đã tồn tại sản phẩm chi tiết tương tự hay chưa
            var spGioHang = await _context.GioHangs.FirstOrDefaultAsync(x => x.Id_KhachHang == tk.Id && x.Id_ChiTietSanPham == spct.Id);
            if (spGioHang == null)
            {
                // Chưa có sản phẩm chi tiết tương tự  thì thêm mới
                _context.GioHangs.Add(new GioHang
                {
                    Id_ChiTietSanPham = spct.Id,
                    Id_KhachHang = tk.Id,
                    SoLuong = spReq.soLuongSp,
                    TrangThai = true
                });
                _context.SaveChanges();
                mess = "Thêm thành công.";
            }
            else
            {
                if ((spGioHang.SoLuong + spReq.soLuongSp) > spct.SoLuong)
                {
                    mess = "Hết hàng.";
                    return Json(new { status = 400, success = false, message = mess });
                }
                // có rồi thì tăng số lượng
                spGioHang.SoLuong += spReq.soLuongSp;
                _context.SaveChanges();
                mess = "Thêm thành công.";

            }
            return Json(new { status = 200, success = true, message = mess });
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
                    return JsonConvert.SerializeObject(resp.KichThuocResps.Where(x => x.Id_MauSac == id).DistinctBy(x => x.Id).ToList());
                }
            }
            return null;
        }


        [HttpPost]
        public async Task<IActionResult> LayDanhSachSanPhamDaXem([FromBody] string[] param)
        {
            var resp = new List<SanPhamDaXemResp>();
            if (param.Length <= 0)
            {
                return Json(new { status = 200, data = "" });

            }
            var lstIdSp = param.Select(x => new
            {
                x = int.Parse(x)
            }).Distinct().Select(x => x.x).ToList();
            var lstSp = await _context.SanPhams.Where(x => lstIdSp.Contains(x.Id))
                .Select(x => new
                {
                    Id = x.Id,
                    Img = _context.HinhAnhs.FirstOrDefault(a => a.Id_SanPham == x.Id).Url ?? "",
                    Ten = x.Ten,
                    GiaBan = Math.Round(x.Gia),
                    GiaBanDau = 0,
                    PhamTramGiamGia = 0,
                    Id_DanhMuc = x.Id_DanhMuc
                }).ToListAsync();
            var lstIdDm = lstSp.Select(x => x.Id_DanhMuc).Distinct().ToList();

            // Kiểm tra các sản phẩm có khuyến mại hay ko
            var timeNow = DateTime.Now;
            var lstKhuyenMaiCT = await _context.ChiTietKhuyenMais.Where(x => lstIdDm.Contains((int)x.Id_DanhMuc))
                .Include(x => x.KhuyenMai)
                .Where(x => x.KhuyenMai.TrangThai == 1)
                .Where(x => x.KhuyenMai.NgayBatDau <= timeNow && timeNow <= x.KhuyenMai.NgayKetThuc)
                .ToListAsync();

            foreach (var item in lstSp)
            {
                var khuyenMai = lstKhuyenMaiCT.FirstOrDefault(x => x.Id_DanhMuc == item.Id_DanhMuc) ?? null;
                if (khuyenMai == null)
                {
                    resp.Add(new SanPhamDaXemResp
                    {
                        Id = item.Id,
                        Ten = item.Ten,
                        Img = item.Img,
                        GiaBan = item.GiaBan,
                        GiaBanDau = item.GiaBanDau,
                        PhamTramGiamGia = item.PhamTramGiamGia,
                    });
                    continue;
                }
                var giaBan = khuyenMai != null && item.GiaBan >= khuyenMai?.KhuyenMai.DieuKienGiamGia ? item.GiaBan - (item.GiaBan * khuyenMai.KhuyenMai.GiaTriGiam / 100) : item.GiaBan;
                var giaBanDau = item.GiaBan;
                var phanTram = khuyenMai.KhuyenMai.GiaTriGiam;
                resp.Add(new SanPhamDaXemResp
                {
                    Id = item.Id,
                    Ten = item.Ten,
                    Img = item.Img,
                    GiaBan = Math.Round(giaBan),
                    GiaBanDau = Math.Round(giaBanDau),
                    PhamTramGiamGia = phanTram,
                });
            }

            return Json(new { status = 200, data = resp });
        }
        public IActionResult ClearSession()
        {
            // Xóa  Session có tên "ChiTietSanPham"
            HttpContext.Session.Remove("ChiTietSanPham");
            return Content("Session đã được xóa.");
        }
    }
}
