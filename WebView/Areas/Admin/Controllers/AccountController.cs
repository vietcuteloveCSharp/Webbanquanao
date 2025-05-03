using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Responses.Responses;
using Responses.Resquests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebView.Repository;
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
     
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JWTToken");
            return RedirectToAction("Login");
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

                   
                    var tokenHandle = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandle.ReadJwtToken(loginResponse.Token);
                    var roleClaim = jwtToken.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Role)?.Value;
                    // Lưu token vào Cookie 
                    Response.Cookies.Append(
                        "JWTToken",
                        loginResponse.Token,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            //Expires = DateTime.Now.AddHours(1)
                        }
                    );
                    if (roleClaim == "admin") 
                    {
                        return RedirectToAction("Index","SanPham");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Bạn không có quyền truy cập");
                        Response.Cookies.Delete("JWTToken");
                        return View();
                    }
                   
                }
                // Xử lý khi đăng nhập thất bại
                ModelState.AddModelError(string.Empty, loginResponse.Message);
                return View();

            }
            ModelState.AddModelError(string.Empty, $"Tên tài khoản hoặc mật khẩu không chính xác ");
            return View();
        }

    }
}
