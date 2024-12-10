using DAL.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]

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
                var entity = new DAL.Entities.KichThuoc
                {
                    Ten = model.Ten
                };

                _context.KichThuocs.Add(entity);
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

                entity.Ten = model.Ten;

                _context.KichThuocs.Update(entity);
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
            TempData["success"] = "Xóa kích thước và thuộc tính thành công!";

            // Chuyển hướng về trang danh sách sản phẩm
            return RedirectToAction("Index");
        }


    }
}
