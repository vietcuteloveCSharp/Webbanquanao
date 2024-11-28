using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SanPhamController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách tất cả các ChiTietSanPham từ cơ sở dữ liệu, bao gồm SanPham, MauSac, và KichThuoc
            var chiTietSanPhams = await _context.ChiTietSanPhams
                .Include(ct => ct.SanPham)
                .Include(ct => ct.MauSac)
                .Include(ct => ct.KichThuoc)
                .ToListAsync();

            return View(chiTietSanPhams);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "Id", "Ten");
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc");
            var sanPham = new SanPham
            {
                NgayTao = DateTime.Now
            };
            // Thu thập lỗi và gán vào ViewBag để hiển thị
            TempData["error"] = "Có lỗi xảy ra khi thêm sản phẩm.";
            List<string> errors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            ViewBag.Errors = errors;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham sanPham)
        {

            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "Id", "Ten",sanPham.Id_ThuongHieu);
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc",sanPham.Id_DanhMuc);
            if (ModelState.IsValid)
            {
                //if (sanPham.ImageUpload != null)
                //{
                //    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                //    if (!Directory.Exists(uploadDir))
                //    {
                //        Directory.CreateDirectory(uploadDir);
                //    }

                //    string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(sanPham.ImageUpload.FileName);
                //    string filePath = Path.Combine(uploadDir, imageName);

                //    using (var fs = new FileStream(filePath, FileMode.Create))
                //    {
                //        await sanPham.ImageUpload.CopyToAsync(fs);
                //    }

                //    sanPham.HinhAnh = imageName;
                //}
                if (sanPham.NgayTao == default(DateTime))
                {
                    sanPham.NgayTao = DateTime.Now;
                }

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Có lỗi xảy ra khi thêm sản phẩm.";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                ViewBag.Errors = errors;
            }


            return View(sanPham);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
                return NotFound();

            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus.ToList(), "Id", "Ten", sanPham.Id_ThuongHieu);
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs.ToList(), "Id", "TenDanhMuc", sanPham.Id_DanhMuc);
            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPham sanPham)
        {
            if (id != sanPham.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Cập nhật sản phẩm thành công";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id))
                        return NotFound();
                    throw;
                }
            }
            else
            {
                TempData["error"] = "Có lỗi xảy ra khi cập nhật sản phẩm.";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                ViewBag.Errors = errors;
            }

            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus.ToList(), "Id", "Ten", sanPham.Id_ThuongHieu);
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs.ToList(), "Id", "TenDanhMuc", sanPham.Id_DanhMuc);
            return View(sanPham);
        }
        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(sp => sp.Id == id);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            SanPham sanPham = await _context.SanPhams.FindAsync(id);

            if (sanPham == null)
                return NotFound();

            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();
            TempData["success"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index");
        }
    }
}
