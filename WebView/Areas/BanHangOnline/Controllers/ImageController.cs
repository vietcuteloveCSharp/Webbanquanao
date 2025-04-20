using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class ImageController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public ImageController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        // 🖼 Trang upload ảnh
        public IActionResult Upload()
        {
            return View("Upload");
        }

        // 🖼 Xử lý upload ảnh vào database
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất một file.");
                return View();
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    var imageData = memoryStream.ToArray();

                    var image = new Image
                    {
                        FileName = file.FileName,
                        ImageData = imageData,
                        ContentType = file.ContentType
                    };

                    _context.Images.Add(image);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Gallery"); // Chuyển đến trang Gallery sau khi upload
        }

        // 📥 Lấy ảnh từ database để hiển thị trên trang
        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            return File(image.ImageData, image.ContentType);
        }

        // 📸 Trang Gallery để hiển thị ảnh đã upload
        public async Task<IActionResult> Gallery()
        {
            var images = await _context.Images.ToListAsync();
            return View(images);
        }
    }
}
