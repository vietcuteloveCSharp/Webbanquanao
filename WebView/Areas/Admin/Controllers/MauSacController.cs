using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MauSacController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public MauSacController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var mauSacs = _context.MauSacs.ToList();

            var mauSacDTOs = mauSacs.Select(m => new MauSacDTO
            {
                Id = m.Id,
                Ten = m.Ten,
                MaHex = m.MaHex
            }).ToList();

            return View(mauSacDTOs);
        }

        // 2. GET: Trang thêm mới màu sắc
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. POST: Xử lý thêm mới màu sắc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MauSacDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = new MauSac
                {
                    Ten = model.Ten,
                    MaHex = model.MaHex
                };

                _context.MauSacs.Add(entity);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm màu sắc mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // 3. GET: Trang chỉnh sửa màu sắc
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.MauSacs.FindAsync(id);
            if (entity == null) return NotFound();

            var model = new MauSacDTO
            {
                Id = entity.Id,
                Ten = entity.Ten,
                MaHex = entity.MaHex
            };

            return View(model);
        }

        // 3. POST: Xử lý chỉnh sửa màu sắc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MauSacDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _context.MauSacs.FindAsync(model.Id);
                if (entity == null) return NotFound();

                entity.Ten = model.Ten;
                entity.MaHex = model.MaHex;

                _context.MauSacs.Update(entity);
                await _context.SaveChangesAsync();
                TempData["success"] = "Cập nhật màu sắc thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // 4. POST: Xóa màu sắc
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.MauSacs.FindAsync(id);
            if (entity == null) return NotFound();

            _context.MauSacs.Remove(entity);
            await _context.SaveChangesAsync();
            TempData["success"] = "Xóa màu sắc thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
