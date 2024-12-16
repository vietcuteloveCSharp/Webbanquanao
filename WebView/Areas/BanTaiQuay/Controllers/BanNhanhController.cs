using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.BanTaiQuay.Controllers
{
    [Area("BanTaiQuay")]
    public class BanNhanhController : Controller
    {
        private WebBanQuanAoDbContext _dbContext;
        public BanNhanhController(WebBanQuanAoDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {

            return View("BanNhanh");
        }

        // API lấy sản phẩm với tìm kiếm và phân trang
        [HttpGet]
        public IActionResult GetProducts(string searchQuery = "", int page = 1, int pageSize = 10)
        {
            var products = new List<SanPham>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = _dbContext.SanPhams.ToList().Where(p => p.Ten.Contains(searchQuery.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var paginatedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArray();

            return Json(new
            {
                Products = paginatedProducts,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery
            });
        }
    }
}
