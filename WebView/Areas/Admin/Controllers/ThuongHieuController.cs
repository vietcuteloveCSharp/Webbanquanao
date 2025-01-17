using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThuongHieuDTO model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên thương hiệu đã tồn tại trong cơ sở dữ liệu chưa
                var existingTen = await _context.ThuongHieus
                    .FirstOrDefaultAsync(t => t.Ten.Trim().ToLower() == model.Ten.Trim().ToLower());

                // Nếu tên đã tồn tại, thêm lỗi vào ModelState
                if (existingTen != null)
                {
                    ModelState.AddModelError("Ten", "Tên thương hiệu đã tồn tại.");
                    return View(model); // Trả về view với lỗi
                }

                // Nếu tên chưa tồn tại, tiếp tục thêm mới
                var entity = new ThuongHieu
                {
                    Ten = model.Ten,
                    MoTa = model.MoTa,
                    TrangThai = model.TrangThai,
                    // Thêm bất kỳ trường nào cần thiết khác vào đây
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
                // Lấy thông tin bản ghi đang cần chỉnh sửa
                var entity = await _context.ThuongHieus.FindAsync(model.Id);
                if (entity == null) return NotFound();

                // Kiểm tra xem tên thương hiệu đã tồn tại trong cơ sở dữ liệu chưa, ngoại trừ bản ghi hiện tại
                var existingTen = await _context.ThuongHieus
                    .FirstOrDefaultAsync(t => t.Ten.Trim().ToLower() == model.Ten.Trim().ToLower() && t.Id != model.Id);

                // Nếu tên đã tồn tại, thêm lỗi vào ModelState
                if (existingTen != null)
                {
                    ModelState.AddModelError("Ten", "Tên thương hiệu đã tồn tại.");
                    return View(model); // Trả về view với lỗi
                }

                // Cập nhật các thuộc tính của thương hiệu
                entity.Ten = model.Ten;
                entity.MoTa = model.MoTa;
                entity.TrangThai = model.TrangThai;

                _context.ThuongHieus.Update(entity);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật thương hiệu thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi validation, trả về form để người dùng sửa lại
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
