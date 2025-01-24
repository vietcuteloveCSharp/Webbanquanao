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
    public class GioHangController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public GioHangController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        // Thông tin giỏ hàng của khách hàng
        public async Task<IActionResult> Index()
        {
            var resp = new List<GioHangResp>();
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Redirect("/BanHangOnline/DangNhapDangKy/ChuaDangNhap");
            }
            // list giỏ hàng của khách hàng
            var lstGioHang = await _context.GioHangs.Where(x => x.Id_KhachHang == tk.Id && x.SoLuong > 0).Include(x => x.ChiTietSanPham).ToListAsync();

            if (lstGioHang == null || lstGioHang.Count <= 0)
            {
                ViewData["SpGioHang"] = new List<GioHangResp>();
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
            // Tìm danh sách Khuyến mại với các sản phẩm trong giỏ hàng
            var lstDanhMucTheoSanPhamGioHang = lstSp.Select(x => x.Id_DanhMuc).Distinct().ToList();
            var timeNow = DateTime.Now;
            var lstKhuyenMai = await _context.ChiTietKhuyenMais.Where(x => lstDanhMucTheoSanPhamGioHang.Contains((int)x.Id_DanhMuc))
                .Include(x => x.KhuyenMai)
                .Where(x => x.KhuyenMai.TrangThai == 1)
                .Where(x => x.KhuyenMai.NgayBatDau <= timeNow && timeNow <= x.KhuyenMai.NgayKetThuc).ToListAsync();
            // tính giá bán và giá ban đầu cho các sản phẩm

            var lstSpNew = new List<SanPhamResp>();
            foreach (var item in lstSp)
            {
                var khuyenMai = lstKhuyenMai?.FirstOrDefault(c => c.Id_DanhMuc == item.Id_DanhMuc)?.KhuyenMai ?? null;
                var giaBan = khuyenMai != null && item.Gia >= khuyenMai.DieuKienGiamGia ? Math.Round(item.Gia - (item.Gia * khuyenMai.GiaTriGiam / 100)) : Math.Round(item.Gia);
                lstSpNew.Add(new SanPhamResp
                {
                    Id = item.Id,
                    GiaBan = giaBan,
                    GiaBanDau = Math.Round(item.Gia),
                    MoTa = item.MoTa,
                    SoLuong = item.SoLuong,
                    Ten = item.Ten,
                    ListHinHAnh = lstSpVoiHinhAnh.FirstOrDefault(b => b.IdSP == item.Id).ListHinHAnh,
                    Id_DanhMuc = item.Id_DanhMuc,
                });
            }
            // Tổng hợp lại toàn bộ dựa trên list giỏ hàng

            resp = lstGioHang.Select(x => new GioHangResp
            {
                Id = x?.Id,
                SoLuong = x?.SoLuong,
                IdChiTietSanPham = x?.Id_ChiTietSanPham,
                IdKichThuoc = x?.ChiTietSanPham?.Id_KichThuoc,
                IdMauSac = x?.ChiTietSanPham?.Id_MauSac,
                KichThuocResps = lstKichThuoc.Where(a => a.IdSp == x.ChiTietSanPham.Id_SanPham && a.IdMs == x.ChiTietSanPham?.Id_MauSac).Select(a => new KichThuocResp
                {
                    Id = a?.Id,
                    Ten = a?.Ten,
                    Id_MauSac = x?.ChiTietSanPham?.Id_MauSac
                }).Distinct().ToList(),
                MauSacResps = lstMauSac.Where(a => a.IdSp == x.ChiTietSanPham.Id_SanPham).Select(x => new MauSacResp
                {
                    Id = x.Id,
                    MaHex = x.MaHex,
                    Ten = x.Ten
                }).Distinct().ToList(),
                SanPham = lstSpNew.Where(a => a.Id == x.ChiTietSanPham.Id_SanPham).FirstOrDefault(),
            }).ToList();
            ViewData["SpGioHang"] = resp;
            HttpContext.Session.Remove("SanPhamGioHang");
            HttpContext.Session.SetString("SanPhamGioHang", JsonConvert.SerializeObject(resp));
            return View();
        }


        // tăng, giảm số lượng sp trong giỏ hàng
        [HttpGet]
        public async Task<IActionResult> GetThayDoiSoLuongGioHang(int idGioHang = 0, bool isTang = true)
        {
            // check nguoi dung phai dang nhap
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            if (idGioHang <= 0)
            {
                return Json(new { status = 403, success = false, message = "Không tìm thấy sản phẩm" });
            }
            var spGioHang = await _context.GioHangs.FirstOrDefaultAsync(x => x.Id_KhachHang == tk.Id && x.Id == idGioHang);
            if (spGioHang == null)
            {
                return Json(new { status = 403, success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }
            var spct = await _context.ChiTietSanPhams.FirstOrDefaultAsync(x => x.Id == spGioHang.Id_ChiTietSanPham);
            if (spct == null)
            {
                return Json(new { status = 403, success = false, message = "không tìm thấy sản phẩm" });
            }
            if (spct.SoLuong == 0)
            {
                //_context.GioHangs.Remove(spGioHang);
                //_context.SaveChanges();
                return Json(new { status = 400, success = false, message = "Sản phẩm đã hết hàng. Xóa sản phẩm khỏi giỏ hàng" });
            }
            if (spct.SoLuong == 1)
            {
                spGioHang.SoLuong = spct.SoLuong;
                _context.SaveChanges();
                return Json(new { status = 400, success = false, message = "Hành động không thành công" });
            }
            if (isTang)
            {
                // kiểm tra không tăng quá số lượng sản phẩm trong giỏ hàng so với sản phẩm chi tiết
                if (spct.SoLuong < spGioHang.SoLuong)
                {
                    spGioHang.SoLuong = spct.SoLuong;
                    _context.SaveChanges();
                    return Json(new { status = 400, success = false, message = "Tăng sản phẩm không thành công" });
                }
                if (spct.SoLuong == spGioHang.SoLuong)
                {
                    return Json(new { status = 400, success = false, message = "Tăng sản phẩm không thành công" });
                }

                spGioHang.SoLuong += 1;
                _context.SaveChanges();
                return Json(new { status = 200, success = true, message = "Tăng sản phẩm thành công" });
            }
            else
            {
                if (spGioHang.SoLuong == 1)
                {
                    return Json(new { status = 400, success = false, message = "Giảm sản phẩm không thành công" });
                }
                spGioHang.SoLuong -= 1;
                _context.SaveChanges();
                return Json(new { status = 200, success = true, message = "Giảm sản phẩm thành công" });
            }
        }

        ///xóa sp khỏi giỏ hàng
        [HttpGet]
        public async Task<IActionResult> GetXoaSanPhamKhoiGioHang(int idGioHang = 0)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            if (idGioHang <= 0)
            {
                return Json(new { status = 403, success = false, message = "Không tìm thấy sản phẩm" });
            }
            var spGioHang = await _context.GioHangs.FirstOrDefaultAsync(x => x.Id_KhachHang == tk.Id && x.Id == idGioHang);
            if (spGioHang == null)
            {
                return Json(new { status = 403, success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }
            _context.GioHangs.Remove(spGioHang);
            _context.SaveChanges();
            return Json(new { status = 200, success = true, message = "Xóa thành công" });
        }
        //thay đổi phân loại giỏ hàng
        [HttpPost]
        public async Task<IActionResult> PostThayDoiPhanLoaiSanPhamTromgGioHang(ThayDoiPhamLoaiGioHangParam request)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            if (request == null || request.IdGioHang == 0 || request.IdMS == 0 || request.IdKT == 0)
                return Json(new { status = 400, success = false, message = "Thay đổi không thành công" });
            var gioHangBanDau = await _context.GioHangs.FirstOrDefaultAsync(x => x.Id_KhachHang == tk.Id && x.Id == request.IdGioHang);
            if (gioHangBanDau == null)
            {
                return Json(new { status = 403, success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }
            var spct = await _context.ChiTietSanPhams.FirstOrDefaultAsync(x => x.Id == gioHangBanDau.Id_ChiTietSanPham);
            if (spct == null)
                return Json(new { status = 403, success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
            var spctNew = await _context.ChiTietSanPhams.FirstOrDefaultAsync(x => x.Id_SanPham == spct.Id_SanPham && x.Id_MauSac == request.IdMS && x.Id_KichThuoc == request.IdKT);
            if (spctNew == null || spctNew.SoLuong == 0)
            {
                return Json(new { status = 403, success = false, message = "Phân loại sản phẩm hết hàng" });
            }
            var sp = await _context.SanPhams.FirstOrDefaultAsync(x => x.Id == spct.Id_SanPham);
            if (_context.GioHangs.Any(x => x.Id_KhachHang == tk.Id && x.Id_ChiTietSanPham == spctNew.Id))
            {
                // th sản phẩm chi tiết mới của kh đã tồn tại trong giỏ hàng thì sẽ cộng thêm số lượng sp cũ trong giỏ hàng vào sp mới && xóa giỏ hàng đó
                // lấy giỏ hàng bị có sản phẩm chi tiết bị trùng lặp
                var giohangTrung = await _context.GioHangs.FirstOrDefaultAsync(x => x.Id_KhachHang == tk.Id && x.Id_ChiTietSanPham == spctNew.Id);
                if (giohangTrung == null)
                {
                    return Json(new { status = 403, success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
                }
                // check số lượng sản phẩm trong giỏ hàng không được lớn hơn sản phẩm
                if (sp.SoLuong < (giohangTrung.SoLuong + gioHangBanDau.SoLuong))
                {
                    giohangTrung.SoLuong = sp.SoLuong;
                }
                giohangTrung.SoLuong += gioHangBanDau.SoLuong;
                _context.GioHangs.Remove(gioHangBanDau);
            }
            else
            {
                // th sản phẩm ct mới chưa có trong giỏ của kh -> thêm mới loại hàng đó với số lượng tương đương && xóa giỏ hàng đó

                gioHangBanDau.Id_ChiTietSanPham = spctNew.Id;
            }
            _context.SaveChanges();
            return Json(new { status = 200, success = true, message = "Thay đổi thành công" });
        }

        // lấy kích thước theo máu sắc sản phẩm
        [HttpGet]
        public async Task<IActionResult> GetKichThuocSanPhamTrongGioHang(int idGioHang = 0, int idMs = 0)
        {
            var resp = new List<KichThuocResp>();
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(resp);
            }
            if (idGioHang <= 0 || idMs <= 0)
            {
                return Json(resp);
            }
            var spGioHang = await _context.GioHangs.FirstOrDefaultAsync(x => x.Id_KhachHang == tk.Id && x.Id == idGioHang);
            if (spGioHang == null)
            {
                return Json(resp);
            }
            var spct = await _context.ChiTietSanPhams.FirstOrDefaultAsync(x => x.Id == spGioHang.Id_ChiTietSanPham);
            if (spct == null)
                return Json(resp);
            var lstSpCTTheoMs = await _context.ChiTietSanPhams.Where(x => x.Id_SanPham == spct.Id_SanPham).Where(x => x.Id_MauSac == idMs).Select(x => x.KichThuoc).ToListAsync();
            if (lstSpCTTheoMs == null || lstSpCTTheoMs.Count <= 0)
            {
                return Json(resp);
            }
            resp = lstSpCTTheoMs.Select(x => new KichThuocResp
            {
                Id = x.Id,
                Id_MauSac = idMs,
                Ten = x.Ten,
            }).OrderBy(x => x.Ten).Distinct().ToList();
            return Json(resp);
        }
    }
}
