using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using WebView.Repository;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class ThanhToanController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public ThanhToanController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetThanhToan()
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            // trả về thông tin cần thanh toán
            return View();
        }
    }
}
