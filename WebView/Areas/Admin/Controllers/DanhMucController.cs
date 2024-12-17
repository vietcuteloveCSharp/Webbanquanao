using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DanhMucController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public DanhMucController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        // 1. GET: Danh sách danh mục
        [HttpGet]
        public IActionResult Index()
        {
            var danhMucs = _context.DanhMucs
                .Select(dm => new DanhMucDTO
                {
                    Id = dm.Id,
                    TenDanhMuc = dm.TenDanhMuc
                   ,TrangThai = dm.TrangThai
                }).ToList();

            return View(danhMucs);
        }
        // 2. GET: Trang thêm mới danh mục
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. POST: Xử lý thêm mới danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMucDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = new DanhMuc
                {
                    TenDanhMuc = model.TenDanhMuc,
                    NgayTao = DateTime.Now, // Ngày tạo mặc định
                    TrangThai = true // Mặc định trạng thái là hoạt động
                };

                _context.DanhMucs.Add(entity);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // 3. GET: Trang chỉnh sửa danh mục
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.DanhMucs.FindAsync(id);
            if (entity == null) return NotFound();

            var model = new DanhMucDTO
            {
                Id_DanhMucCha = entity.Id_DanhMucCha,
                TenDanhMuc = entity.TenDanhMuc,
                NgayTao = entity.NgayTao,
                TrangThai = entity.TrangThai
            };

            return View(model);
        }

        // 3. POST: Xử lý chỉnh sửa danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DanhMucDTO model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _context.DanhMucs.FindAsync(model.Id);
                if (entity == null) return NotFound();

                // Cập nhật các thuộc tính
                entity.TenDanhMuc = model.TenDanhMuc;
                entity.TrangThai = model.TrangThai;

                _context.DanhMucs.Update(entity);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model); // Trả lại form nếu có lỗi
        }


        public async Task<IActionResult> Delete(int id)
        {
            // Tìm danh mục cần xóa
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null) return NotFound();

            // Xóa các bản ghi liên quan trong bảng ChiTietKhuyenMais
            var chiTietKhuyenMais = _context.ChiTietKhuyenMais.Where(c => c.Id_DanhMuc == id);
            _context.ChiTietKhuyenMais.RemoveRange(chiTietKhuyenMais);

            // Xóa danh mục
            _context.DanhMucs.Remove(danhMuc);
            await _context.SaveChangesAsync();

            TempData["success"] = "Xóa danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

    }
}
