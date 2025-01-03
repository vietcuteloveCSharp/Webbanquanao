using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Sanphams;
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
                    LoaiKhuyenMai = km.LoaiKhuyenMai,
                    GiaTriGiam = km.GiaTriGiam,
                    DieuKienGiamGia = km.DieuKienGiamGia,
                    NgayTao = km.NgayTao,
                    NgayBatDau = km.NgayBatDau,
                    NgayKetThuc = km.NgayKetThuc,
                    TrangThai = km.TrangThai,
                    ChiTietKhuyenMaiDTOs = km.ChiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiDTO
                    {
                        Id_KhuyenMai = ct.Id_KhuyenMai,
                        Id_DanhMuc = ct.Id_DanhMuc,
                    }).ToList()
                }).ToListAsync();

            return View(khuyenMaiDtos); // Truyền nguyên danh sách KhuyenMaiDTO
        }
        public async Task PopulateDropDownLists(KhuyenMaiDTO khuyenMaiDTO = null)
        {
            var danhMucs = await _context.DanhMucs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.TenDanhMuc
            }).ToListAsync();

            ViewBag.DanhMucs = danhMucs;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var khuyenmaiDTO = _context.KhuyenMais.ToList();

            return View(khuyenmaiDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KhuyenMaiDTO khuyenMaiDTO)
        {
            if (ModelState.IsValid)
            {
                var khuyenMai = new KhuyenMai
                {
                    Ten = khuyenMaiDTO.Ten,
                    MoTa = khuyenMaiDTO.MoTa,
                    GiaTriGiam = khuyenMaiDTO.GiaTriGiam,
                    DieuKienGiamGia = khuyenMaiDTO.DieuKienGiamGia,
                    NgayTao = khuyenMaiDTO.NgayTao,
                    NgayBatDau = khuyenMaiDTO.NgayBatDau,
                    NgayKetThuc = khuyenMaiDTO.NgayKetThuc,
                    TrangThai = khuyenMaiDTO.TrangThai
                };

                await _context.KhuyenMais.AddAsync(khuyenMai);
                await _context.SaveChangesAsync();

                // Thêm ChiTietKhuyenMai nếu có
                if (khuyenMaiDTO.ChiTietKhuyenMaiDTOs != null && khuyenMaiDTO.ChiTietKhuyenMaiDTOs.Any())
                {
                    foreach (var ct in khuyenMaiDTO.ChiTietKhuyenMaiDTOs)
                    {
                        _context.ChiTietKhuyenMais.Add(new ChiTietKhuyenMai
                        {
                            Id_KhuyenMai = khuyenMai.Id,
                            Id_DanhMuc = ct.Id_DanhMuc
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(khuyenMaiDTO);
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
