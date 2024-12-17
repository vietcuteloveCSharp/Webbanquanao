using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThuongHieuController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public ThuongHieuController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        // 1. GET: Danh sách thương hiệu
        [HttpGet]
        public IActionResult Index()
        {
            var thuongHieus = _context.ThuongHieus
                .Select(th => new ThuongHieuDTO
                {
                    Id = th.Id,
                    Ten = th.Ten,
                    MoTa = th.MoTa,
                    TrangThai = th.TrangThai
                }).ToList();

            return View(thuongHieus);
        }

        // 2. GET: Trang thêm mới thương hiệu
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. POST: Xử lý thêm mới thương hiệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThuongHieuDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = new ThuongHieu
                {
                    Ten = model.Ten,
                    MoTa = model.MoTa,
                    TrangThai = model.TrangThai,
                    // Thêm bất kỳ trường nào cần thiết khác vào đây.
                };

                _context.ThuongHieus.Add(entity);
                await _context.SaveChangesAsync();

                TempData["success"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu validation không thành công, trở lại form.
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.ThuongHieus.FindAsync(id);
            if (entity == null) return NotFound();

            var model = new ThuongHieuDTO
            {
                Id = entity.Id,
                Ten = entity.Ten,
                MoTa = entity.MoTa,
                TrangThai = entity.TrangThai
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ThuongHieuDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _context.ThuongHieus.FindAsync(model.Id);
                if (entity == null) return NotFound();

                // Cập nhật các thuộc tính
                entity.Ten = model.Ten;
                entity.MoTa = model.MoTa;
                entity.TrangThai = model.TrangThai;

                _context.ThuongHieus.Update(entity);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật thương hiệu thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Trả lại form nếu có lỗi
            return View(model);
        }

        // 4. GET: Xóa thương hiệu
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.ThuongHieus.FindAsync(id);
            if (entity == null) return NotFound();

            // Xóa thương hiệu
            _context.ThuongHieus.Remove(entity);
            await _context.SaveChangesAsync();

            TempData["success"] = "Xóa thương hiệu thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
