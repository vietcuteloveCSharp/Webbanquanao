using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class ThanhToanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
