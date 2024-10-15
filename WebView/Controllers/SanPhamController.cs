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
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            // Truy vấn sản phẩm và bao gồm các thuộc tính liên quan như ChiTietSanPham, MauSac và KichThuoc
            var sanPham = await _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ctsp => ctsp.MauSac) // Bao gồm thông tin Màu Sắc
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ctsp => ctsp.KichThuoc) // Bao gồm thông tin Kích Thước
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            // Lấy các sản phẩm liên quan
            var relatedProducts = await _context.SanPhams
                .Where(p => p.Id_DanhMuc == sanPham.Id_DanhMuc && p.Id != id)
                .Take(9)
                .ToListAsync();

            // Gán danh sách sản phẩm liên quan vào ViewBag
            ViewBag.RelatedProducts = relatedProducts;

            // Trả về view với chi tiết sản phẩm
            return View(sanPham.ChiTietSanPhams);
        }



    }
}
