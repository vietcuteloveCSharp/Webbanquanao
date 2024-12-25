using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class TimKiemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
