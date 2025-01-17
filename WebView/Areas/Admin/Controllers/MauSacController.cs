using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MauSacDTO model)
        {
            // Loại bỏ khoảng trắng thừa và chuyển tất cả về chữ thường trước khi so sánh
            var trimmedTen = model.Ten?.Trim().ToLower();
            var trimmedMaHex = model.MaHex?.Trim().ToLower();

            // Kiểm tra xem tên màu sắc đã tồn tại trong cơ sở dữ liệu chưa (so sánh tên không phân biệt chữ hoa chữ thường và bỏ qua khoảng trắng)
            var existingTen = await _context.MauSacs
                .FirstOrDefaultAsync(m => m.Ten.Trim().ToLower() == trimmedTen);

            // Kiểm tra xem mã Hex đã tồn tại trong cơ sở dữ liệu chưa (so sánh mã Hex không phân biệt chữ hoa chữ thường và bỏ qua khoảng trắng)
            var existingMaHex = await _context.MauSacs
                .FirstOrDefaultAsync(m => m.MaHex.Trim().ToLower() == trimmedMaHex);

            // Nếu trùng tên
            if (existingTen != null)
            {
                ModelState.AddModelError("Ten", "Tên màu sắc đã tồn tại.");
            }

            // Nếu trùng mã Hex
            if (existingMaHex != null)
            {
                ModelState.AddModelError("MaHex", "Mã màu đã tồn tại.");
            }

            // Nếu không có lỗi, tiếp tục thêm mới
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
                MaHex = entity.MaHex,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MauSacDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _context.MauSacs.FindAsync(model.Id);
                if (entity == null) return NotFound();

                // Loại bỏ khoảng trắng thừa và chuyển tất cả về chữ thường trước khi so sánh
                var trimmedTen = model.Ten?.Trim().ToLower();
                var trimmedMaHex = model.MaHex?.Trim().ToLower();

                // Kiểm tra xem tên màu sắc đã tồn tại trong cơ sở dữ liệu chưa (trừ bản ghi hiện tại)
                var existingTen = await _context.MauSacs
                    .FirstOrDefaultAsync(m => m.Ten.Trim().ToLower() == trimmedTen && m.Id != model.Id);

                // Kiểm tra xem mã Hex đã tồn tại trong cơ sở dữ liệu chưa (trừ bản ghi hiện tại)
                var existingMaHex = await _context.MauSacs
                    .FirstOrDefaultAsync(m => m.MaHex.Trim().ToLower() == trimmedMaHex && m.Id != model.Id);

                // Nếu trùng tên
                if (existingTen != null)
                {
                    ModelState.AddModelError("Ten", "Tên màu sắc đã tồn tại.");
                }

                // Nếu trùng mã Hex
                if (existingMaHex != null)
                {
                    ModelState.AddModelError("MaHex", "Mã màu đã tồn tại.");
                }

                // Nếu không có lỗi, tiếp tục cập nhật
                if (ModelState.IsValid)
                {
                    entity.Ten = model.Ten;
                    entity.MaHex = model.MaHex;

                    _context.MauSacs.Update(entity);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Cập nhật màu sắc thành công!";
                    return RedirectToAction(nameof(Index));
                }
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
