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
            var lst5DM = Random5DM(lstDm);
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
            var lstSp = await _context.SanPhams.Include(x => x.ChiTietSanPhams).Where(x => lstIdSp.Contains(x.Id)).Where(x => x.ChiTietSanPhams != null && x.ChiTietSanPhams.Count != 0).ToListAsync();
            var lstHinhAnh = await _context.HinhAnhs.Where(x => lstIdSp.Contains((int)x.Id_SanPham)).ToListAsync();

            // resp

            foreach (var item in lst5DM)
            {
                var lstSpResp = lstSp.Where(x => x.Id_DanhMuc == item.Id).Select(x => new SanPhamResp
                {
                    Id = x.Id,
                    GiaBan = x.Gia,
                    GiaBanDau = 0,
                    MoTa = x.MoTa,
                    SoLuong = x?.SoLuong,
                    Ten = x?.Ten,
                    Id_DanhMuc = x?.Id_DanhMuc,
                    ListHinHAnh = lstHinhAnh.Where(a => a.Id_SanPham == x.Id)?.Select(a => new HinhAnhResp
                    {
                        Id = a?.Id,
                        Url = a?.Url,
                        Id_SanPham = a?.Id_SanPham,
                        ImageData = a?.ImageData,
                        ImageSourceType = a.ImageSourceType,
                    }).ToList()
                }).ToList();
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
