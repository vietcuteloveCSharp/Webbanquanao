using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Responses.Responses;
using Responses.Resquests;
using System.Text;
using WebView.Services;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;
        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginResquest data)
        {
            string apiUrl = "https://localhost:7169/api/Account/Login"; 

            var loginRequest = new LoginResquest
            {
                TaiKhoan = data.TaiKhoan,
                MatKhau = data.MatKhau,
            };

            // Gọi API đăng nhập để lấy JWT
            var response = await _apiService.PostAsync(apiUrl, loginRequest, string.Empty);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<ResponseText>(jsonData);
                if (loginResponse.Success)
                {
                    // Lưu token JWT vào session 
                    HttpContext.Session.SetString("JWTToken", loginResponse.Token);
                    // Xử lý thành công, có thể chuyển hướng hoặc trả về dữ liệu
                    return RedirectToAction("Index", "SanPham");
                    // Chuyển hướng tới trang sản phẩm admin
                }
                // Xử lý khi đăng nhập thất bại
                ModelState.AddModelError(string.Empty,loginResponse.Message);
                return View();
            }
            ModelState.AddModelError(string.Empty, "Error logging in");
            return RedirectToAction("Index", "Order");
        }

    }
}
