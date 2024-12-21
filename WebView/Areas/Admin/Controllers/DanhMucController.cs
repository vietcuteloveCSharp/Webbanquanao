using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        // GET: Form tạo danh mục
        [HttpGet]
        public IActionResult Create()
        {
            var danhMucs = _context.DanhMucs.ToList();
            ViewBag.DanhMucs = new SelectList(danhMucs, "Id", "TenDanhMuc"); // Đảm bảo trạng thái danh mục được chọn đúng
            return View();
        }


        // 2. POST: Xử lý thêm mới danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMucDTO model)
        {
            // Kiểm tra xem ModelState có hợp lệ không
            if (ModelState.IsValid)
            {
                var entity = new DanhMuc
                {
                    TenDanhMuc = model.TenDanhMuc,
                    NgayTao = DateTime.Now, // Ngày tạo mặc định
                    TrangThai = model.TrangThai // Trạng thái sẽ lấy từ model, nếu không chọn sẽ có giá trị mặc định true
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
