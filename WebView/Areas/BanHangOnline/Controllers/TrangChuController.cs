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
                    ListHinHAnh = lstHinhAnh.Where(a => a.Id_SanPham == x.Id).Select(a => new HinhAnhResp
                    {
                        Id = a?.Id,
                        Url = a?.Url,
                        Id_SanPham = a?.Id_SanPham,
                        ImageData = a?.ImageData
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
            if (param != null || param.Count > 0)
            {
                int count = 0;
                int length = param.Count;
                List<int> arrRandom = new List<int>();
                List<DanhMuc> resp = new List<DanhMuc>();
                do
                {
                    count++;
                    Random random = new Random();

                    int randomNumber = random.Next(0, length);
                    if (arrRandom.Count >= 0 && arrRandom.Any(x => x == randomNumber))
                    {
                        count--;
                        continue;
                    }
                    resp.Add(param[randomNumber]);
                    arrRandom.Add(randomNumber);
                    if (count == 5)
                    {
                        return resp;
                    }
                }
                while (true);
            }
            return null;
        }
    }
}
