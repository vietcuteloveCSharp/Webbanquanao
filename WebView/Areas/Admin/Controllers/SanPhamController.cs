using DAL.Context;
using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public SanPhamController(WebBanQuanAoDbContext dbcontext)
        {
            _context = dbcontext;
        }
        public IActionResult Index()
        {
            var products = _context.SanPhams.ToList();
            return View(products);
        }
    }
}
