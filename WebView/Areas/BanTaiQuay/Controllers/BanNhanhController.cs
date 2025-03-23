using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;
using WebView.Areas.BanTaiQuay.DTOBanTaiQuay.Param;
using WebView.Areas.BanTaiQuay.DTOBanTaiQuay.Resp;

namespace WebView.Areas.BanTaiQuay.Controllers
{
    [Area("BanTaiQuay")]
    public class BanNhanhController : Controller
    {
        private WebBanQuanAoDbContext _dbContext;
        public BanNhanhController(WebBanQuanAoDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {

            return View("BanNhanh");
        }

        // API lấy sản phẩm với tìm kiếm và phân trang
        [HttpGet]
        public async Task<IActionResult> GetProducts(string searchQuery = "", int page = 1, int pageSize = 10)
        {
            var resp = new List<SanPhamBanTaiQuayResp>();
            //var products = new List<SanPhamResp>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var lstSp = _dbContext.SanPhams.ToList().Where(p => p.Ten.Trim().ToLower().Contains(searchQuery.Trim().ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
                // tìm khuyến mại theo sản phẩm và hiển thị
                var lstIdDm = lstSp.Select(x => x.Id_DanhMuc).ToList();
                var timenow = DateTime.Now;
                var lstKhuyenMai = await _dbContext.ChiTietKhuyenMais.Where(x => lstIdDm.Contains((int)x.Id_DanhMuc))
                    .Include(x => x.KhuyenMai)
                    .Where(x => x.KhuyenMai.TrangThai == 1)
                    .Where(x => x.KhuyenMai.NgayBatDau <= timenow && timenow <= x.KhuyenMai.NgayKetThuc)
                    .ToListAsync();
                if (lstSp.Count > 0)
                {
                    foreach (var item in lstSp)
                    {
                        var khuyenMai = lstKhuyenMai.FirstOrDefault(x => x.Id_DanhMuc == item.Id_DanhMuc);
                        decimal giaBan = 0;

                        giaBan = khuyenMai != null && item.Gia >= khuyenMai?.KhuyenMai?.DieuKienGiamGia ? Math.Round(item.Gia - (item.Gia * khuyenMai.KhuyenMai.GiaTriGiam / 100)) : Math.Round(item.Gia);

                        // lấy màu sắc + kích thước + số lượng trong Sản phẩm chi tiết
                        var lstSpCt = _dbContext.ChiTietSanPhams.Where(x => x.Id_SanPham == item.Id)
                            .Include(x => x.MauSac).Include(x => x.KichThuoc)
                            .Where(x => x.SoLuong > 0)
                            .ToList();
                        if (lstSpCt != null && lstSpCt.Count > 0)
                        {
                            foreach (var spct in lstSpCt)
                            {
                                resp.Add(new SanPhamBanTaiQuayResp
                                {
                                    Id = spct.Id,
                                    GiaBan = giaBan,
                                    GiaBanDau = Math.Round(item.Gia),
                                    Id_DanhMuc = item.Id_DanhMuc,
                                    MoTa = item.MoTa,
                                    SoLuong = spct.SoLuong,
                                    Ten = item.Ten,
                                    MauSac = new MauSacResp
                                    {
                                        Id = spct?.MauSac?.Id,
                                        MaHex = spct?.MauSac?.MaHex,
                                        Ten = spct?.MauSac?.Ten
                                    },
                                    KichThuoc = new KichThuocResp
                                    {
                                        Id = spct?.KichThuoc?.Id,
                                        Ten = spct?.KichThuoc?.Ten
                                    },
                                    ListHinHAnh = _dbContext.HinhAnhs.Where(x => x.Id_SanPham == item.Id)
                                    .Take(1)?.Select(x => new HinhAnhResp
                                    {
                                        Id = x.Id,
                                        Id_SanPham = x.Id_SanPham,
                                        //ImageData = x.ImageData,
                                        ImageSourceType = x.ImageSourceType,
                                        Url = x.Url
                                    })?.ToList() ?? null,
                                });
                            }
                        }
                    }
                }
            }
            int totalProducts = resp.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var paginatedProducts = resp
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArray();

            return Json(new
            {
                Products = paginatedProducts,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetKhachHang(string sdt = "")
        {
            if (string.IsNullOrEmpty(sdt))
            {
                return Json(new
                {
                    KhachHangs = "",
                    SearchQuery = sdt
                });
            }
            var kh = await _dbContext.KhachHangs.Where(x => x.Sdt.ToLower().Trim().Contains(sdt.ToLower().Trim()))?.Take(6)
                .Select(x => new
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    Sdt = x.Sdt,
                    Email = x.Email,
                    TrangThai = x.TrangThai
                }).ToListAsync();
            return Json(new
            {
                KhachHangs = kh,
                SearchQuery = sdt
            });
        }

        [HttpPost]
        public async Task<IActionResult> PostThanhToanTaiQuay([FromBody] ThanhToanTaiQuayParam req)
        {
            if (req != null)
            {
                var khachHang = new KhachHang();
                if (req.idkhachang != "0")
                {
                    khachHang = _dbContext.KhachHangs.Where(x => x.Id == int.Parse(req.idkhachang)).FirstOrDefault();

                }
                else
                {
                    khachHang = null;
                }

                var lstIdspct = req.lsthoadon.Select(x => int.Parse(x.id)).ToList();
                var lstSpChiTiet = await _dbContext.ChiTietSanPhams.Where(x => lstIdspct.Contains(x.Id)).Include(x => x.SanPham).ToListAsync();
                if (lstSpChiTiet != null && lstSpChiTiet.Count > 0)
                {
                    var lstHoadon = await _dbContext.HoaDons.ToListAsync();
                    var hoadonDb = _dbContext.HoaDons.Add(new HoaDon
                    {
                        Id_KhachHang = khachHang != null ? khachHang.Id : null,
                        PhiVanChuyen = 0,
                        TongTien = req.tongtienthanhtoan,
                        TrangThai = Enum.EnumVVA.ETrangThaiHD.HoanThanhDon,
                        DiaChiGiaoHang = "",
                    });
                    _dbContext.SaveChanges();
                    var hoadon = hoadonDb.Entity;
                    // cập nhật số lượng trong sản phẩm + chi tiết sản phẩm
                    foreach (var item in lstSpChiTiet)
                    {
                        var spct = req.lsthoadon.FirstOrDefault(x => int.Parse(x.id) == item.Id);
                        item.SoLuong -= spct != null ? spct.quantity : 0;
                        item.SanPham.SoLuong -= spct != null ? spct.quantity : 0;
                        if (item.SoLuong < 0)
                        {
                            item.SoLuong = 0;
                        }
                        if (item.SanPham.SoLuong < 0)
                        {
                            item.SanPham.SoLuong = 0;
                        }
                        _dbContext.ChiTietHoaDons.Add(new ChiTietHoaDon
                        {
                            Id_HoaDon = hoadon.Id,
                            SoLuong = spct.quantity,
                            Id_ChiTietSanPham = item.Id,
                            Gia = spct.gia,
                            TrangThai = true,
                        });
                    }
                    // lưu thông tin thanh toán vào db
                    var checkPttt = _dbContext.PhuongThucThanhToans.Any(x => x.Ten.ToLower().Trim() == "taiquay");
                    var tt = new PhuongThucThanhToan();
                    if (!checkPttt)
                    {
                        await addnewPhuongThucTT();
                    }
                    var ttthanhtoan = _dbContext.PhuongThucThanhToans.FirstOrDefault(x => x.Ten.ToLower().Trim() == "taiquay");
                    _dbContext.ThanhToanHoaDons.Add(new ThanhToanHoaDon
                    {
                        Id_HoaDon = hoadon.Id,
                        MaGiaoDich = "",
                        TongTien = req.tongtienhang,
                        SoTienDaThanhToan = req.tongtienthanhtoan,
                        PhuongThucThanhToan = ttthanhtoan,
                        Id_PhuongThucThanhToan = ttthanhtoan.Id,
                        NgayThanhToan = DateTime.Now,
                        HoaDon = hoadon
                    });
                    _dbContext.SaveChanges();
                    return Json(new
                    {
                        message = "Thành công",
                        status = 200
                    });
                }
                return Json(new
                {
                    message = "Thất bại",
                    status = 400
                });
            }
            else
            {
                return Json(new
                {
                    message = "Thất bại",
                    status = 400
                });
            }

        }
        private async Task<bool> addnewPhuongThucTT()
        {
            var pptt = new PhuongThucThanhToan
            {
                ThanhToanHoaDons = null,
                NgayTao = DateTime.Now,
                Mota = "Thanh toán tại quầy",
                Ten = "taiquay",
                TrangThai = true,
            };
            await _dbContext.PhuongThucThanhToans.AddAsync(pptt);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
