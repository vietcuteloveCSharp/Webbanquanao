using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Controllers
{
    public class ThuongHieuController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public ThuongHieuController(WebBanQuanAoDbContext dbcontext)
        {
            _context = dbcontext;
        }
        public async Task<IActionResult> Index(int id)
        {
            var thuonghieu = await _context.ThuongHieus.FindAsync(id);
            if (thuonghieu == null)
            {
                return NotFound();
            }

           
            var products = await _context.SanPhams
                .Where(p => p.Id_ThuongHieu == id)
                .ToListAsync();

            return View(products); // Trả về danh sách sản phẩm
        }
       
    }
}
