using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrderController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public List<HoaDon> GetListHoaDon()
        {
            // TODO: Implement logic to retrieve the list of HoaDon from the database or any other source
            List<HoaDon> listHoaDon = new List<HoaDon>();
            // Add code to populate the listHoaDon
            return listHoaDon;
        }
        public IActionResult Index()
        {
            List<HoaDon> listHoaDon = GetListHoaDon();
            return View(listHoaDon);
        }

       
    }
}
