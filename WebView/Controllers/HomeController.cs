using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebView.Extensions;
using WebView.Models;

namespace WebView.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly ILogger<HomeController> _logger;

        // Lớp này có thể gọi là dạng middleware
        private readonly GetHttpClient _httpClient;
        // B2: Khai báo trong constructor 1 biến có cùng kiểu Class GetHttpClient
        public HomeController(ILogger<HomeController> logger, WebBanQuanAoDbContext dbcontext, GetHttpClient httpClient)
        {
            _logger = logger;
            _context = dbcontext;
            _httpClient = httpClient;
        }
        // Mẫu gọi 1 api
        public async Task<IActionResult> Test()
        {
            //B3: Gọi method GetPublichHttpClient (dùng để gọi api public) hoặc GetPrivateHttpClient (sử dụng để phân quyền - chưa cần sử dụng ngay)
            var http = _httpClient.GetPrivateHttpClient();
            //B4: gọi api cần thiết GET, POST, UPDATE,.... tùy vào api cần gọi là gì
            // Backend api: https://localhost:7169/WeatherForecast
            // -> Client : method Get và đường dẫn: WeatherForecast
            var getListWeather = await http.GetAsync($"WeatherForecast");
            // Lấy 1 kết quả nhiệt độ 
            // Backend api: https://localhost:7169/WeatherForecast/GetWeatherForecastByTemp?temp=66
            // Client: method Get và đường dẫn: WeatherForecast/GetWeatherForecastByTemp?temp=66
            var temp = 24;
            var getByTemp = await http.GetAsync($"WeatherForecast/GetWeatherForecastByTemp?temp={temp}");
            //B5: Lấy kết quả từ Prop của biến client trên và chuyển đổi thành đúng kiểu dữ liệu, nếu ko đúng kiểu sẽ ra dự liệu dạng khác.
            var listWeather = getListWeather.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
            var weather = getByTemp.Content.ReadFromJsonAsync<WeatherForecast>();
            // sử dụng debug để xem kết quả 
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var sanpham = _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ct => ct.MauSac)  // Nạp dữ liệu MauSac
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ct => ct.KichThuoc)  // Nạp dữ liệu KichThuoc
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.ThuongHieu)
                .ToList();

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
