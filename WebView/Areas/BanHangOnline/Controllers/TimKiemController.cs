using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class TimKiemController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public TimKiemController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTimKiemSanPham(string? str = "")
        {
            var resp = new List<SanPhamTimKiemResp>();
            if (string.IsNullOrEmpty(str))
            {
                ViewData["ClientSessionData"] = null;
                return View("Index", resp);
            }
            // tìm toàn bộ sản phâm có tên chứa ký tự str, trạng thái hoạt động, số lượng >=1
            // ưu tiên: sản phẩm mới, đang trong đợt khuyến mại
            var lstSp = await _context.SanPhams.AsNoTracking().Where(x => x.TrangThai == true)
                                                                .Where(x =>x.Ten.ToLower().Trim().Contains(str.ToLower().Trim()))
                                                                .Include(x => x.DanhMuc).Include(x => x.ChiTietSanPhams).ThenInclude(a => a.KichThuoc)
                                                                .Include(x => x.ChiTietSanPhams).ThenInclude(a => a.MauSac)
                                                                .Where(x => x.TrangThai == true && x.ChiTietSanPhams.Any(a => a.SoLuong >= 1)).ToListAsync();
            if (lstSp == null || lstSp.Count <= 0)
            {
                ViewData["ClientSessionData"] = null;
                return View("Index", resp);
            }
            // kiểm tra có bất kỳ đợt khuyến mại. Khuyến mại áp dụng theo danh mục
            var lstIdDanhMuc = lstSp.Select(x => x.Id_DanhMuc).Distinct().ToList();
            // list chi tiết khuyến mại
            DateTime timeNow = DateTime.Now;
            var lstChiTietKhuyenMai = await _context.ChiTietKhuyenMais.Include(x => x.KhuyenMai).Where(x => lstIdDanhMuc.Contains((int)x.Id_DanhMuc)).Where(x => x.KhuyenMai.TrangThai == 1).Where(x => x.KhuyenMai.NgayBatDau.CompareTo(timeNow) <= 0).Where(x => !(x.KhuyenMai.NgayKetThuc != null) || timeNow <= x.KhuyenMai.NgayKetThuc).Include(x => x.KhuyenMai.chiTietKhuyenMais).ToListAsync();
            // thực hiện lọc khuyến mại theo lstIdDanhMuc
            var listChiTietKMMoi = new List<ChiTietKhuyenMai>();
            // lọc list chi tiết khuyến mại theo cái tạo mới nhất
            if (lstChiTietKhuyenMai != null && lstChiTietKhuyenMai.Count >= 1)
            {
                foreach (var iddm in lstIdDanhMuc)
                {
                    var lst = lstChiTietKhuyenMai.Where(x => x.Id_DanhMuc == iddm).OrderByDescending(x => x.Id).ToList();
                    if (lst != null && lst.Count > 0)
                    {
                        listChiTietKMMoi.AddRange(lst);
                    }

                }
            }
            // list SanPhamTimKiemResp
            foreach (var iddm in lstIdDanhMuc)
            {
                var km = listChiTietKMMoi?.FirstOrDefault(a => a.Id_DanhMuc == iddm)?.KhuyenMai;
                var dkGiam = km?.DieuKienGiamGia ?? 0;
                var giaTriGiam = km?.GiaTriGiam ?? 0;
                resp.AddRange(lstSp.Where(x => x.Id_DanhMuc == iddm).Select(x => new SanPhamTimKiemResp
                {
                    ChiTietSanPhams = new()
                    {
                        SanPham = new SanPhamResp()
                        {
                            Id = x.Id,
                            GiaBan = x.Gia >= dkGiam && giaTriGiam > 0 ? Math.Round(x.Gia - (x.Gia * giaTriGiam / 100)) : Math.Round(x.Gia),
                            GiaBanDau = Math.Round(x.Gia),
                            Id_DanhMuc = x.Id_DanhMuc,
                            MoTa = x.MoTa,
                            SoLuong = x.ChiTietSanPhams.Sum(a => a.SoLuong),
                            Ten = x.Ten,
                            ListHinHAnh = _context.HinhAnhs.Where(a => a.Id_SanPham == x.Id).Select(a => new HinhAnhResp
                            {
                                Id = a.Id,
                                Id_SanPham = a.Id_SanPham,
                                ImageData = a.ImageData,
                                ImageSourceType = a.ImageSourceType,
                                Url = a.Url
                            }).Take(1).ToList()
                        },
                        KichThuocResps = x.ChiTietSanPhams.Where(x => x.TrangThai).Select(a => new KichThuocResp
                        {
                            Id = a.KichThuoc.Id,
                            Ten = a.KichThuoc.Ten,
                        }).Distinct().ToList(),
                        MauSacResps = x.ChiTietSanPhams.Where(x => x.TrangThai).Select(a => new MauSacResp
                        {
                            Id = a.MauSac.Id,
                            MaHex = a.MauSac.MaHex,
                            Ten = a.MauSac.Ten
                        }).Distinct().ToList(),
                    },
                    DanhMucs = new DanhMucResp
                    {
                        Id = x.DanhMuc.Id,
                        Id_DanhMucCha = x.DanhMuc.Id_DanhMucCha,
                        NgayTao = x.DanhMuc.NgayTao,
                        TenDanhMuc = x.DanhMuc.TenDanhMuc,
                        TrangThai = x.DanhMuc.TrangThai,
                    }
                }).OrderByDescending(x => x.ChiTietSanPhams.SanPham.Id).ToList());
            }
            // Sắp xếp giá từ thấp đến cao
            if (resp == null || resp.Count <= 0)
            {
                ViewData["ClientSessionData"] = null;
                return View("Index", resp);
            }
            resp = resp.OrderBy(x => x.ChiTietSanPhams.SanPham.GiaBan).ToList();
            var serila = JsonSerializer.Serialize(resp);
            ViewData["ClientSessionData"] = serila;
            return View("Index", resp);
        }
    }


}
