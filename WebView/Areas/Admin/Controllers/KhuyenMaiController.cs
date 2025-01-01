using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhuyenMaiController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public KhuyenMaiController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbcontext;
        }
        // GET: Danh sách khuyến mại
        public async Task<IActionResult> Index()
        {
            var khuyenMaiDtos = await _context.KhuyenMais
                .Include(km => km.ChiTietKhuyenMais)
                .Select(km => new KhuyenMaiDTO
                {
                    Id = km.Id,
                    Ten = km.Ten,
                    MoTa = km.MoTa,
                    NgayTao = km.NgayTao,
                    NgayBatDau = km.NgayBatDau,
                    NgayKetThuc = km.NgayKetThuc,
                    TrangThai = km.TrangThai,
                    ChiTietKhuyenMaiDTOs = km.ChiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiDTO
                    {
                        Id = ct.Id,
                        LoaiKhuyenMai = ct.LoaiKhuyenMai,
                        GiaTriGiam = ct.GiaTriGiam,
                        MenhGia = ct.MenhGia,
                        GiaTriToiDa = ct.GiaTriToiDa,
                        Id_DanhMuc = ct.Id_DanhMuc,
                    }).ToList()
                }).ToListAsync();

            return View(khuyenMaiDtos.SelectMany(km => km.ChiTietKhuyenMaiDTOs));
        }

        // GET: Tạo mới khuyến mại
        public IActionResult Create()
        {
            var danhMucs = _context.DanhMucs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.TenDanhMuc
            }).ToList();

            ViewBag.DanhMucs = danhMucs;

            return View(new KhuyenMaiDTO());
        }

        // POST: Tạo mới khuyến mại
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMaiDTO dto)
        {
            if (ModelState.IsValid)
            {
                var khuyenMai = new KhuyenMai
                {
                    Ten = dto.Ten,
                    MoTa = dto.MoTa,
                    NgayTao = DateTime.Now,
                    NgayBatDau = dto.NgayBatDau,
                    NgayKetThuc = dto.NgayKetThuc,
                    TrangThai = dto.TrangThai
                };

                // Thêm ChiTietKhuyenMai
                khuyenMai.ChiTietKhuyenMais = dto.ChiTietKhuyenMaiDTOs.Select(ct => new ChiTietKhuyenMai
                {
                    LoaiKhuyenMai = ct.LoaiKhuyenMai,
                    GiaTriGiam = ct.GiaTriGiam,
                    MenhGia = ct.MenhGia,
                    GiaTriToiDa = ct.GiaTriToiDa,
                    Id_DanhMuc = ct.Id_DanhMuc
                }).ToList();

                _context.Add(khuyenMai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DanhMucs"] = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc", dto.ChiTietKhuyenMaiDTOs.FirstOrDefault()?.Id_DanhMuc);

            return View(dto);
        }
        // GET: Sửa khuyến mại
        public async Task<IActionResult> Edit(int id)
        {
            var khuyenMai = await _context.KhuyenMais
                .Include(km => km.ChiTietKhuyenMais)
                .FirstOrDefaultAsync(km => km.Id == id);

            if (khuyenMai == null) return NotFound();

            var dto = new KhuyenMaiDTO
            {
                Id = khuyenMai.Id,
                Ten = khuyenMai.Ten,
                MoTa = khuyenMai.MoTa,
                NgayTao = khuyenMai.NgayTao,
                NgayBatDau = khuyenMai.NgayBatDau,
                NgayKetThuc = khuyenMai.NgayKetThuc,
                TrangThai = khuyenMai.TrangThai,
                ChiTietKhuyenMaiDTOs = khuyenMai.ChiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiDTO
                {
                    Id = ct.Id,
                    LoaiKhuyenMai = ct.LoaiKhuyenMai,
                    GiaTriGiam = ct.GiaTriGiam,
                    MenhGia = ct.MenhGia,
                    GiaTriToiDa = ct.GiaTriToiDa,
                    Id_DanhMuc = ct.Id_DanhMuc
                }).ToList()
            };

            ViewBag.DanhMucs = _context.DanhMucs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.TenDanhMuc
            }).ToList();

            return View(dto);
        }

        // POST: Sửa khuyến mại
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhuyenMaiDTO dto)
        {
            if (id != dto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var khuyenMai = await _context.KhuyenMais
                    .Include(km => km.ChiTietKhuyenMais)
                    .FirstOrDefaultAsync(km => km.Id == id);

                if (khuyenMai == null) return NotFound();

                khuyenMai.Ten = dto.Ten;
                khuyenMai.MoTa = dto.MoTa;
                khuyenMai.NgayBatDau = dto.NgayBatDau;
                khuyenMai.NgayKetThuc = dto.NgayKetThuc;
                khuyenMai.TrangThai = dto.TrangThai;

                // Xóa các chi tiết khuyến mại cũ
                _context.ChiTietKhuyenMais.RemoveRange(khuyenMai.ChiTietKhuyenMais);

                // Thêm các chi tiết mới
                khuyenMai.ChiTietKhuyenMais = dto.ChiTietKhuyenMaiDTOs.Select(ct => new ChiTietKhuyenMai
                {
                    LoaiKhuyenMai = ct.LoaiKhuyenMai,
                    GiaTriGiam = ct.GiaTriGiam,
                    MenhGia = ct.MenhGia,
                    GiaTriToiDa = ct.GiaTriToiDa,
                    Id_DanhMuc = ct.Id_DanhMuc
                }).ToList();

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DanhMucs = _context.DanhMucs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.TenDanhMuc    
            }).ToList();

            return View(dto);
        }

    }
}
