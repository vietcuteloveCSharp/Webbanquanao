using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class DangNhapDangKyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult XacNhanOtp()
        {
            return View("XacNhanOtp");
        }
    }
}
