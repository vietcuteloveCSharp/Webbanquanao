using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChucVuController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public ChucVuController(WebBanQuanAoDbContext dbContext)
        {
            _context = dbContext;
        }

        // 📌 Hiển thị danh sách chức vụ
        public async Task<IActionResult> Index()
        {
            var chucVus = await _context.ChucVus
                .Select(cv => new WebView.NghiaDTO.ChucVuDTO
                {
                    Id = cv.Id,
                    Ten = cv.Ten,
                    Mota = cv.Mota
                }).ToListAsync();

            return View(chucVus);
        }

        // 📌 Form tạo chức vụ
        public IActionResult Create()
        {
            return View();
        }

        // 📌 Xử lý thêm chức vụ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WebView.NghiaDTO.ChucVuDTO model)
        {
            if (ModelState.IsValid)
            {
                var chucVu = new ChucVu
                {
                    Ten = model.Ten,
                    Mota = model.Mota
                };
                _context.ChucVus.Add(chucVu);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm chức vụ thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Thêm chức vụ thất bại!";
            return View(model);
        }

        // 📌 Form cập nhật chức vụ
        public async Task<IActionResult> Edit(int id)
        {
            var chucVu = await _context.ChucVus.FindAsync(id);
            if (chucVu == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy chức vụ!";
                return NotFound();
            }

            var model = new WebView.NghiaDTO.ChucVuDTO
            {
                Id = chucVu.Id,
                Ten = chucVu.Ten,
                Mota = chucVu.Mota
            };

            return View(model);
        }

        // 📌 Xử lý cập nhật chức vụ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WebView.NghiaDTO.ChucVuDTO model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ!";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var chucVu = await _context.ChucVus.FindAsync(id);
                if (chucVu == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy chức vụ!";
                    return NotFound();
                }

                chucVu.Ten = model.Ten;
                chucVu.Mota = model.Mota;

                _context.ChucVus.Update(chucVu);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật chức vụ thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Cập nhật chức vụ thất bại!";
            return View(model);
        }

        // 📌 Xử lý xóa chức vụ
        public async Task<IActionResult> Delete(int id)
        {
            var chucVu = await _context.ChucVus.FindAsync(id);
            if (chucVu == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy chức vụ!";
                return NotFound();
            }

            _context.ChucVus.Remove(chucVu);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa chức vụ thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
