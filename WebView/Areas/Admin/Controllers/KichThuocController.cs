using DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]

    public class KichThuocController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public KichThuocController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbcontext;
        }
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var kichThuocs = _context.KichThuocs.ToList();

            var kichThuocDTOs = kichThuocs.Select(k => new NghiaDTO.KichThuocDTO
            {
                // Map từng thuộc tính
                Id = k.Id,
                Ten = k.Ten,
                // Thêm các thuộc tính khác nếu cần
            }).ToList();

            // Trả về view với danh sách đã map
            return View(kichThuocDTOs);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NghiaDTO.KichThuocDTO model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đã tồn tại trong cơ sở dữ liệu chưa
                var existingTen = await _context.KichThuocs
                    .FirstOrDefaultAsync(k => k.Ten.Trim().ToLower() == model.Ten.Trim().ToLower());

                // Nếu tên đã tồn tại, thêm lỗi vào ModelState
                if (existingTen != null)
                {
                    ModelState.AddModelError("Ten", "Tên kích thước đã tồn tại.");
                    return View(model); // Trả về view với lỗi
                }

                // Nếu tên chưa tồn tại, tiếp tục thêm mới
                var entity = new DAL.Entities.KichThuoc
                {
                    Ten = model.Ten
                };

                _context.KichThuocs.Add(entity);
                TempData["success"] = "Thêm kích thước thành công!";

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu không hợp lệ, trả về view cùng dữ liệu lỗi
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.KichThuocs.FindAsync(id);
            if (entity == null) return NotFound();

            var model = new NghiaDTO.KichThuocDTO
            {
                Id = entity.Id,
                Ten = entity.Ten
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NghiaDTO.KichThuocDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _context.KichThuocs.FindAsync(model.Id);
                if (entity == null) return NotFound();

                // Loại bỏ khoảng trắng thừa và chuyển tất cả về chữ thường trước khi so sánh
                var trimmedTen = model.Ten?.Trim().ToLower();

                // Kiểm tra xem tên kích thước đã tồn tại trong cơ sở dữ liệu chưa (trừ bản ghi hiện tại)
                var existingTen = await _context.KichThuocs
                    .FirstOrDefaultAsync(k => k.Ten.Trim().ToLower() == trimmedTen && k.Id != model.Id);

                // Nếu tên đã tồn tại, thêm lỗi vào ModelState
                if (existingTen != null)
                {
                    ModelState.AddModelError("Ten", "Tên kích thước đã tồn tại.");
                    return View(model); // Trả về view cùng dữ liệu lỗi
                }

                // Nếu tên chưa tồn tại, tiếp tục cập nhật
                entity.Ten = model.Ten;

                _context.KichThuocs.Update(entity);
                TempData["success"] = "Sửa kích thước thành công!";

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // 4. DELETE: Xóa một mục
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.KichThuocs.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.KichThuocs.Remove(entity);
            await _context.SaveChangesAsync();
            // Thêm thông báo thành công
            TempData["success"] = "Xóa kích thước thành công!";

            // Chuyển hướng về trang danh sách sản phẩm
            return RedirectToAction("Index");
        }


    }
}
