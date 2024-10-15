using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebView.Models;

namespace WebView.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, WebBanQuanAoDbContext dbcontext)
        {
            _logger = logger;
            _context = dbcontext; 
        }

        public IActionResult Index()
        {
            var sanpham = _context.SanPhams.Include("DanhMuc").Include("ThuongHieu").ToList();
            return View(sanpham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult GoHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
