using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class ChiTietSanPhamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
