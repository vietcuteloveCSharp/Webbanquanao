using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShiftController : Controller
    {
        public ShiftController()
        {
        }
        public IActionResult ShiftView()
        {
            return View();
        }
    }
}
