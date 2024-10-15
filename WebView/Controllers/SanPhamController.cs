using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public SanPhamController(WebBanQuanAoDbContext dbcontext)
        {
            _context = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _context.SanPhams.ToListAsync();
            return View(sanPhams);
        }
        // Action để lấy thông tin chi tiết sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            // Lấy thông tin chi tiết sản phẩm bao gồm sản phẩm, màu sắc và kích thước
            var chiTietSanPham = await _context.ChiTietSanPhams
                .Include(ctsp => ctsp.SanPham)  // Load thông tin sản phẩm
                .ThenInclude(sp => sp.DanhMuc)  // Load thông tin danh mục sản phẩm
                .Include(ctsp => ctsp.MauSac)   // Load thông tin màu sắc
                .Include(ctsp => ctsp.KichThuoc) // Load thông tin kích thước
                .FirstOrDefaultAsync(ctsp => ctsp.Id == id);

            if (chiTietSanPham == null)
            {
                return NotFound();
            }

            // Lấy sản phẩm liên quan theo danh mục
            ViewBag.RelatedProducts = await _context.SanPhams
                .Where(p => p.Id_DanhMuc == chiTietSanPham.SanPham.Id_DanhMuc && p.Id != chiTietSanPham.SanPham.Id)
                .Take(6) // Giới hạn số lượng sản phẩm liên quan
                .ToListAsync();

            return View(chiTietSanPham);
        }





    }
}
