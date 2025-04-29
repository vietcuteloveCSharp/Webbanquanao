using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.Areas.BanHangOnline.HoangDTO.Param;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;
using WebView.Repository;

[Area("BanHangOnline")]
public class DanhGiaController : Controller
{
    private readonly WebBanQuanAoDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public DanhGiaController(WebBanQuanAoDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // 🖼 Trang danh sách đánh giá sản phẩm
    // lấy thông tin các hóa đơn có trạng thái hoàn thành
    public async Task<IActionResult> DanhSachDanhGia()
    {
        var resp = new List<DanhGiaSanPhamResp>();
        var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
        if (tk == null)
        {
            return Redirect("/BanHangOnline/DangNhapdangky/ChuaDangNhap");
        }
        var lstHoaDonChiTietDaHoanThanh = await _context.ChiTietHoaDons.AsNoTracking()
            .Include(x => x.HoaDon)
            .Where(x => x.HoaDon.Id_KhachHang == tk.Id)
            .Where(x => x.HoaDon.TrangThai == Enum.EnumVVA.ETrangThaiHD.HoanThanhDon)
            .Include(x => x.ChiTietSanPham)
            .ToListAsync();
        if (lstHoaDonChiTietDaHoanThanh == null || lstHoaDonChiTietDaHoanThanh.Count == 0)
        {
            ViewData["lstDanhSachDanhGia"] = resp;
            return View("DanhSachDanhGia");
        }
        var lstIdcthd = lstHoaDonChiTietDaHoanThanh.Select(x => x.Id).ToList();
        var lstIdChiTietsp = lstHoaDonChiTietDaHoanThanh.Select(x => x.Id_ChiTietSanPham).Distinct().ToList();

        var lstSpChiTiet = await _context.ChiTietSanPhams.AsNoTracking()
                                .Where(x => lstIdChiTietsp.Contains(x.Id))
                                .Include(x => x.SanPham)
                                .Include(x => x.MauSac)
                                .Include(x => x.KichThuoc)
                                .ToListAsync();
        var lstSanPhamDaDanhGia = await _context.DanhGias
                                    .Where(x => lstIdcthd.Contains(x.Id_ChiTietHoaDon)).ToListAsync();
        foreach (var item in lstHoaDonChiTietDaHoanThanh)
        {
            var spct = lstSpChiTiet.Where(x => x.Id == item.Id_ChiTietSanPham).Select(x => new ChiTietSanPhamResp
            {
                SanPham = new()
                {
                    Id = x?.SanPham?.Id,
                    Ten = x?.SanPham?.Ten,
                    MoTa = x?.SanPham?.MoTa,
                    ListHinHAnh = _context.HinhAnhs.Where(a => a.Id_SanPham == item.ChiTietSanPham.Id_SanPham).Select(a => new HinhAnhResp
                    {
                        Id = a.Id,
                        Id_SanPham = a.Id_SanPham,
                        Url = a.Url
                    }).ToList()
                },
                KichThuocResps = new List<KichThuocResp> {
                        new KichThuocResp
                        {
                            Id = x?.KichThuoc?.Id,
                            Ten = x?.KichThuoc?.Ten,
                        }
                    },
                MauSacResps = new List<MauSacResp> {
                        new MauSacResp {
                            Id = x?.MauSac?.Id,
                            MaHex = x?.MauSac?.MaHex,
                            Ten = x?.MauSac?.Ten,
                        }
                    }
            }).First();
            var sp = new DanhGiaSanPhamResp
            {
                IdChiTietHoaDon = item.Id,
                ChiTietSanPham = spct,
                TrangThai = lstSanPhamDaDanhGia.Count != 0 && lstSanPhamDaDanhGia.Any(b => b.Id_ChiTietHoaDon == item.Id) ? true : false,
                Sao = lstSanPhamDaDanhGia.Count != 0 && lstSanPhamDaDanhGia.Any(b => b.Id_ChiTietHoaDon == item.Id) ? lstSanPhamDaDanhGia.First(b => b.Id_ChiTietHoaDon == item.Id).Sao : 0,
                NoiDung = lstSanPhamDaDanhGia?.FirstOrDefault(b => b.Id_ChiTietHoaDon == item.Id)?.NoiDung
            };
            resp.Add(sp);
        }
        ViewData["lstDanhSachDanhGia"] = resp;
        return View("DanhSachDanhGia");
    }

    // id của đơn hàng chi tiết đã hoàn thành
    [HttpGet]
    public async Task<IActionResult> ChiTietDanhGia(int id)
    {
        var resp = new DanhGiaSanPhamResp();
        var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
        if (tk == null)
        {
            return Redirect("/BanHangOnline/DangNhapdangky/ChuaDangNhap");
        }
        var hoaDonChiTietDaHoanThanh = await _context.ChiTietHoaDons.AsNoTracking()
           .Where(x => x.Id == id)
           .Include(x => x.HoaDon)
           .Where(x => x.HoaDon.Id_KhachHang == tk.Id)
           .Where(x => x.HoaDon.TrangThai == Enum.EnumVVA.ETrangThaiHD.HoanThanhDon)
           .Include(x => x.ChiTietSanPham)
           .FirstOrDefaultAsync();
        if (hoaDonChiTietDaHoanThanh == null)
        {
            return Redirect("/BanHangOnline/DanhGia/DanhSachDanhGia");
        }

        var spct = await _context.ChiTietSanPhams.AsNoTracking()
                                .Where(x => x.Id == hoaDonChiTietDaHoanThanh.Id_ChiTietSanPham)
                                .Include(x => x.SanPham)
                                .Include(x => x.MauSac)
                                .Include(x => x.KichThuoc)
                                .FirstOrDefaultAsync();
        var lstSanPhamDaDanhGia = await _context.DanhGias.Where(x => x.Id_ChiTietHoaDon == id).FirstOrDefaultAsync();
        if (lstSanPhamDaDanhGia != null)
        {
            return Redirect("/BanHangOnline/DanhGia/DanhSachDanhGia");
        }
        resp = new DanhGiaSanPhamResp
        {
            IdChiTietHoaDon = id,
            ChiTietSanPham = new ChiTietSanPhamResp
            {
                SanPham = new()
                {
                    Id = spct?.SanPham?.Id,
                    Ten = spct?.SanPham?.Ten,
                    MoTa = spct?.SanPham?.MoTa,
                    ListHinHAnh = _context.HinhAnhs.Where(a => a.Id_SanPham == spct.SanPham.Id).Select(a => new HinhAnhResp
                    {
                        Id = a.Id,
                        Id_SanPham = a.Id_SanPham,
                        Url = a.Url
                    }).ToList()
                },
                KichThuocResps = new List<KichThuocResp> {
                        new KichThuocResp
                        {
                            Id = spct?.KichThuoc?.Id,
                            Ten = spct?.KichThuoc?.Ten,
                        }
                    },
                MauSacResps = new List<MauSacResp> {
                        new MauSacResp {
                            Id = spct?.MauSac?.Id,
                            MaHex = spct?.MauSac?.MaHex,
                            Ten = spct?.MauSac?.Ten,
                        }
                    }
            },
            TrangThai = lstSanPhamDaDanhGia != null ? true : false,
        };
        ViewData["spdanhgia"] = resp;

        return View("ChiTietDanhGia");
    }
    // id của đơn hàng chi tiết đã hoàn thành
    [HttpPost]
    public async Task<IActionResult> ChiTietDanhGia([FromForm] DanhGiaSanPhamParam req)
    {

        var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
        if (tk == null)
        {
            return Redirect("/BanHangOnline/DangNhapdangky/ChuaDangNhap");
        }
        var hoaDonChiTietDaHoanThanh = await _context.ChiTietHoaDons.AsNoTracking()
           .Where(x => x.Id == req.idhdct)
           .Include(x => x.HoaDon)
           .Where(x => x.HoaDon.Id_KhachHang == tk.Id)
           .Where(x => x.HoaDon.TrangThai == Enum.EnumVVA.ETrangThaiHD.HoanThanhDon)
           .Include(x => x.ChiTietSanPham)
           .FirstOrDefaultAsync();
        if (hoaDonChiTietDaHoanThanh == null)
        {
            return Redirect("/BanHangOnline/DanhGia/DanhSachDanhGia");
        }

        var spct = await _context.ChiTietSanPhams.AsNoTracking()
                                .Where(x => x.Id == hoaDonChiTietDaHoanThanh.Id_ChiTietSanPham)
                                .Include(x => x.SanPham)
                                .Include(x => x.MauSac)
                                .Include(x => x.KichThuoc)
                                .FirstOrDefaultAsync();
        var lstSanPhamDaDanhGia = await _context.DanhGias.Where(x => x.Id_ChiTietHoaDon == req.idhdct).FirstOrDefaultAsync();
        if (lstSanPhamDaDanhGia != null)
        {
            return Redirect("/BanHangOnline/DanhGia/DanhSachDanhGia");
        }
        var danhgia = _context.DanhGias.Add(new DanhGia
        {
            Id_ChiTietHoaDon = req.idhdct,
            Id_KhachHang = tk.Id,
            NgayTao = DateTime.Now,
            NoiDung = req.noidung,
            Sao = req.sao,
            TrangThai = 1
        }).Entity;
        _context.SaveChanges();
        foreach (var file in req.images)
        {
            if (file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var image = new HinhAnh
                {
                    Id_DanhGia = danhgia.Id,
                    NgayTao = DateTime.Now,
                    FileName = file.FileName,
                    ImageData = imageData,
                    ContentType = file.ContentType,
                    ImageSourceType = 0
                };

                _context.HinhAnhs.Add(image);
                _context.SaveChanges();
            }
        }
        return Redirect("/BanHangOnline/DanhGia/DanhSachDanhGia");
    }

    [HttpGet]
    public async Task<IActionResult> XemChiTiet(int id)
    {
        var resp = new DanhGiaSanPhamResp();
        var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
        if (tk == null)
        {
            return Redirect("/BanHangOnline/DangNhapdangky/ChuaDangNhap");
        }
        var hoaDonChiTietDaHoanThanh = await _context.ChiTietHoaDons.AsNoTracking()
           .Where(x => x.Id == id)
           .Include(x => x.HoaDon)
           .Where(x => x.HoaDon.Id_KhachHang == tk.Id)
           .Where(x => x.HoaDon.TrangThai == Enum.EnumVVA.ETrangThaiHD.HoanThanhDon)
           .Include(x => x.ChiTietSanPham)
           .FirstOrDefaultAsync();
        if (hoaDonChiTietDaHoanThanh == null)
        {
            return Redirect("/BanHangOnline/DanhGia/DanhSachDanhGia");
        }

        var spct = await _context.ChiTietSanPhams.AsNoTracking()
                                .Where(x => x.Id == hoaDonChiTietDaHoanThanh.Id_ChiTietSanPham)
                                .Include(x => x.SanPham)
                                .Include(x => x.MauSac)
                                .Include(x => x.KichThuoc)
                                .FirstOrDefaultAsync();
        var spdadanhgia = await _context.DanhGias.Where(x => x.Id_ChiTietHoaDon == id && x.TrangThai == 1).FirstOrDefaultAsync();
        if (spdadanhgia == null)
        {
            return Redirect("/BanHangOnline/DanhGia/DanhSachDanhGia");
        }
        var lstHinhAnh = await _context.HinhAnhs.Where(x => x.Id_DanhGia == spdadanhgia.Id).Select(x => x.Id).ToListAsync();

        resp = new DanhGiaSanPhamResp
        {
            IdChiTietHoaDon = id,
            ChiTietSanPham = new ChiTietSanPhamResp
            {
                SanPham = new()
                {
                    Id = spct?.SanPham?.Id,
                    Ten = spct?.SanPham?.Ten,
                    MoTa = spct?.SanPham?.MoTa,
                    ListHinHAnh = _context.HinhAnhs.Where(a => a.Id_SanPham == spct.SanPham.Id).Select(a => new HinhAnhResp
                    {
                        Id = a.Id,
                        Id_SanPham = a.Id_SanPham,
                        Url = a.Url
                    }).ToList()
                },
                KichThuocResps = new List<KichThuocResp> {
                        new KichThuocResp
                        {
                            Id = spct?.KichThuoc?.Id,
                            Ten = spct?.KichThuoc?.Ten,
                        }
                    },
                MauSacResps = new List<MauSacResp> {
                        new MauSacResp {
                            Id = spct?.MauSac?.Id,
                            MaHex = spct?.MauSac?.MaHex,
                            Ten = spct?.MauSac?.Ten,
                        }
                    }
            },
            TrangThai = true,
            NoiDung = spdadanhgia.NoiDung,
            Sao = spdadanhgia.Sao,
            HinhAnhs = lstHinhAnh
        };
        ViewData["spdanhgia"] = resp;

        return View("XemChiTiet");
    }

    [HttpGet]
    public async Task<IActionResult> GetImage(int id)
    {
        var image = await _context.HinhAnhs.Where(x => x.Id == id).FirstAsync();
        if (image == null) return NotFound();

        return File(image.ImageData, image.ContentType);
    }


}
