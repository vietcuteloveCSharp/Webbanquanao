using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
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
        // GET: Form tạo danh mục
        [HttpGet]
        public IActionResult Create()
        {
            var danhMucs = _context.DanhMucs.ToList();
            ViewBag.DanhMucs = new SelectList(danhMucs, "Id", "TenDanhMuc"); // Đảm bảo trạng thái danh mục được chọn đúng
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMucDTO model)
        {
            // Kiểm tra xem ModelState có hợp lệ không
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên danh mục đã tồn tại trong cơ sở dữ liệu chưa
                var existingTenDanhMuc = await _context.DanhMucs
                    .FirstOrDefaultAsync(d => d.TenDanhMuc.Trim().ToLower() == model.TenDanhMuc.Trim().ToLower());

                // Nếu tên đã tồn tại, thêm lỗi vào ModelState
                if (existingTenDanhMuc != null)
                {
                    ModelState.AddModelError("TenDanhMuc", "Tên danh mục đã tồn tại.");
                    return View(model); // Trả về view với lỗi
                }

                // Nếu tên chưa tồn tại, tạo mới danh mục
                var entity = new DanhMuc
                {
                    TenDanhMuc = model.TenDanhMuc,
                    NgayTao = DateTime.Now, // Ngày tạo mặc định
                    TrangThai = model.TrangThai // Trạng thái sẽ lấy từ model
                };

                // Thêm vào cơ sở dữ liệu
                _context.DanhMucs.Add(entity);
                await _context.SaveChangesAsync();

                // Hiển thị thông báo thành công
                TempData["success"] = "Thêm danh mục mới thành công!";
                return RedirectToAction(nameof(Index)); // Quay lại danh sách
            }

            // Nếu có lỗi trong model, trả về lại view với model cũ
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DanhMucDTO model)
        {
            if (ModelState.IsValid)
            {
                // Lấy bản ghi hiện tại từ cơ sở dữ liệu
                var entity = await _context.DanhMucs.FindAsync(model.Id);
                if (entity == null) return NotFound();

                // Kiểm tra xem tên danh mục đã tồn tại trong cơ sở dữ liệu chưa, ngoại trừ bản ghi hiện tại
                var existingTenDanhMuc = await _context.DanhMucs
                    .FirstOrDefaultAsync(d => d.TenDanhMuc.Trim().ToLower() == model.TenDanhMuc.Trim().ToLower() && d.Id != model.Id);

                // Nếu tên đã tồn tại, thêm lỗi vào ModelState
                if (existingTenDanhMuc != null)
                {
                    ModelState.AddModelError("TenDanhMuc", "Tên danh mục đã tồn tại.");
                    return View(model); // Trả về view với lỗi
                }

                // Cập nhật các thuộc tính của danh mục
                entity.TenDanhMuc = model.TenDanhMuc;
                entity.TrangThai = model.TrangThai;

                // Cập nhật vào cơ sở dữ liệu
                _context.DanhMucs.Update(entity);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi trong model, trả về lại form với lỗi
            return View(model);
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
