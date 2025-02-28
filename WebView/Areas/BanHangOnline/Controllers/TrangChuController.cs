using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class TrangChuController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public TrangChuController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //ViewData["Danhmuctitle"] = UtilitiClass.GetDanhMucSanPham();
            DictionarySanPhamResp resp = new DictionarySanPhamResp();
            resp.DictionarySanPham = new Dictionary<DanhMucResp, List<SanPhamResp>>();
            //  lấy danh mục phải có ít nhất 3 sản phẩm trở lên
            var lstDm = await _context.DanhMucs.Include(x => x.SanPhams).Where(x => x.SanPhams != null).Where(x => x.SanPhams.Count >= 3).ToListAsync();
            //var lst5DM = Random5DM(lstDm);
            var lst5DM = lstDm?.TakeLast(5)?.ToList() ?? null;
            if (lst5DM == null)
            {
                return View();
            }
            // lấy list sản phẩm
            List<SanPham> lstSpTheoDm = new List<SanPham>();
            lst5DM.ForEach(x => { lstSpTheoDm.AddRange(x?.SanPhams); });
            lstSpTheoDm = lstSpTheoDm.Distinct().ToList();
            // lấy list hình ảnh
            var lstIdSp = lstSpTheoDm.Select(x => x.Id)?.Distinct()?.ToList();
            var lstSp = await _context.SanPhams.Include(x => x.ChiTietSanPhams)
                .Where(x => lstIdSp.Contains(x.Id))
                .Where(x => x.SoLuong > 0)
                .Where(x => x.ChiTietSanPhams != null && x.ChiTietSanPhams.Count != 0)
                .ToListAsync();
            var lstHinhAnh = await _context.HinhAnhs.Where(x => lstIdSp.Contains((int)x.Id_SanPham)).ToListAsync();
            // lấy list khuyến mại theo danh mục
            var timeNow = DateTime.Now;
            var lstIdDm = lst5DM.Select(x => x.Id)?.Distinct()?.ToList();
            var lstKhuyenMaiCT = await _context.ChiTietKhuyenMais.Where(x => lstIdDm.Contains((int)x.Id_DanhMuc))
                .Include(x => x.KhuyenMai)
                .Where(x => x.KhuyenMai.TrangThai == 1)
                .Where(x => x.KhuyenMai.NgayBatDau <= timeNow && timeNow <= x.KhuyenMai.NgayKetThuc)
                .ToListAsync();
            // resp

            foreach (var item in lst5DM)
            {
                var lstSpResp = new List<SanPhamResp>();
                var khuyenMai = lstKhuyenMaiCT?.FirstOrDefault(x => x.Id_DanhMuc == item.Id);
                foreach (var sp in lstSp)
                {
                    if (sp.Id_DanhMuc == item.Id)
                    {
                        // Lấy giá bán đã khuyến mại
                        var giaBan = khuyenMai != null && sp.Gia >= khuyenMai?.KhuyenMai.DieuKienGiamGia ? sp.Gia - (sp.Gia * khuyenMai.KhuyenMai.GiaTriGiam / 100) : sp.Gia;

                        lstSpResp.Add(new SanPhamResp
                        {
                            Id = sp.Id,
                            GiaBan = Math.Round(giaBan),
                            GiaBanDau = Math.Round(sp.Gia),
                            MoTa = sp.MoTa,
                            SoLuong = sp?.SoLuong,
                            Ten = sp?.Ten,
                            Id_DanhMuc = sp?.Id_DanhMuc,
                            ListHinHAnh = lstHinhAnh.Where(a => a.Id_SanPham == sp.Id)?.Select(a => new HinhAnhResp
                            {
                                Id = a?.Id,
                                Url = a?.Url,
                                Id_SanPham = a?.Id_SanPham,
                                ImageData = a?.ImageData,
                                ImageSourceType = a.ImageSourceType,
                            }).ToList()
                        });
                    }
                }
                var DanhMucResp = new DanhMucResp
                {
                    Id = item?.Id,
                    Id_DanhMucCha = item?.Id_DanhMucCha,
                    NgayTao = item?.NgayTao,
                    TenDanhMuc = item?.TenDanhMuc,
                    TrangThai = item?.TrangThai,
                };
                resp.DictionarySanPham[DanhMucResp] = lstSpResp;
            }
            ViewData["lstSanPham"] = resp;
            return View();
        }

        private List<DanhMuc> Random5DM(List<DanhMuc> param)
        {
            if (param == null || param.Count == 0)
            {
                return null;
            }

            int maxCount = Math.Min(5, param.Count); // Xử lý trường hợp danh sách có ít hơn 5 phần tử
            List<int> usedIndices = new List<int>();
            List<DanhMuc> selectedItems = new List<DanhMuc>();
            Random random = new Random();

            while (selectedItems.Count < maxCount)
            {
                int randomIndex = random.Next(0, param.Count); // Tạo chỉ số ngẫu nhiên
                if (!usedIndices.Contains(randomIndex)) // Đảm bảo chỉ số không trùng lặp
                {
                    usedIndices.Add(randomIndex);
                    selectedItems.Add(param[randomIndex]);
                }
            }

            return selectedItems;
        }

    }
}
