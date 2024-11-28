using DAL.Context;
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
    }
}
