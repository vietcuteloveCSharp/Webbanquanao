using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MaGiamGiaController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public MaGiamGiaController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbcontext;
        }
        // GET: Admin/MaGiamGia
        public async Task<IActionResult> Index()
        {
            var maGiamGiaList = await _context.MaGiamGias
                .Include(m => m.ChiTietMaGiamGias)
                .Select(m => new MaGiamGiaDTO
                {
                    Id = m.Id,
                    Ten = m.Ten,
                    LoaiGiamGia = m.LoaiGiamGia,
                    DieuKienGiamGia = m.DieuKienGiamGia,
                    GiaTriGiam = m.GiaTriGiam,
                    MenhGia = m.MenhGia,
                    NoiDung = m.NoiDung,
                    GiaTriToiDa = m.GiaTriToiDa,
                    TrangThai = m.TrangThai,
                    ThoiGianTao = m.ThoiGianTao,
                    ThoiGianKetThuc =m.ThoiGianKetThuc,
                    ChiTietMaGiamGiaDTOs = m.ChiTietMaGiamGias.Select(c => new ChiTietMaGiamGiaDTO
                    {
                        Id = c.Id,
                        Id_KhachHang = c.Id_KhachHang,
                        NoiDung = c.NoiDung,
                        NgaySuDung = c.NgaySuDung,
                        Id_MaGiamGia = c.Id_MaGiamGia
                    }).ToList()
                })
                .ToListAsync();

            return View(maGiamGiaList);
        }
        [HttpGet]

        // GET: Admin/MaGiamGia/Create
        public IActionResult Create()
        {
            var magiamgia = _context.MaGiamGias.ToList();
            return View();
        }
        // POST: Admin/MaGiamGia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaGiamGiaDTO maGiamGiaDto)
        {
            if (ModelState.IsValid)
            {
                var entity = new MaGiamGia
                {
                    Id = maGiamGiaDto.Id,
                    Ten = maGiamGiaDto.Ten,
                    LoaiGiamGia = maGiamGiaDto.LoaiGiamGia,
                    DieuKienGiamGia = maGiamGiaDto.DieuKienGiamGia,
                    GiaTriGiam = maGiamGiaDto.GiaTriGiam,
                    MenhGia = maGiamGiaDto.MenhGia,
                    NoiDung = maGiamGiaDto.NoiDung,
                    GiaTriToiDa = maGiamGiaDto.GiaTriToiDa,
                    TrangThai = maGiamGiaDto.TrangThai,
                    ThoiGianTao = maGiamGiaDto.ThoiGianTao,
                    ThoiGianKetThuc = maGiamGiaDto.ThoiGianKetThuc,
                    // Thêm bất kỳ trường nào cần thiết khác vào đây.
                };
                // Gán thời gian tạo
                _context.MaGiamGias.Add(entity);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(maGiamGiaDto);
        }



    }
}
