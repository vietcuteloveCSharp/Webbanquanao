using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public DanhMucController(WebBanQuanAoDbContext dbcontext)
        {
            _context = dbcontext;
        }
        public async Task<IActionResult> Index(int id)
        {
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            // Lấy danh sách sản phẩm dựa trên danh mục
            var products = await _context.SanPhams
                .Where(p => p.Id_DanhMuc == id)
                .ToListAsync();

            return View(products); // Trả về danh sách sản phẩm
        }
      
    }
}
