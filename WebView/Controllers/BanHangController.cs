using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebView.Controllers
{
    public class BanHangController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public BanHangController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        // Phương thức để hiển thị giao diện bán hàng tại quầy
        public async Task<IActionResult> Index()
        {
            ViewBag.HideSlider = true;
            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var products = await _context.SanPhams.ToListAsync();
            return View(products); // Trả về view BanHang với danh sách sản phẩm
        }
    }
}
