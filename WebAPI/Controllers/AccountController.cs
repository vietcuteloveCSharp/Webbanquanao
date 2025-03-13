using AutoMapper;
using Azure;
using DAL.Context;
using DTO.VuvietanhDTO.NhanViens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Responses.Resquests;
using Service.VuVietAnhService.IRepository.IAccount;
using Service.VuVietAnhService.Repository.Account;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountSerivce;
        private readonly IConfiguration _configuration;
        private readonly WebBanQuanAoDbContext _context;
        public AccountController(WebBanQuanAoDbContext dbContext , IAccountService accountSerivce, IConfiguration configuration)
        {
            _accountSerivce = accountSerivce;
            _configuration = configuration;
            _context = dbContext;

        }
        
        [HttpPost("RegisterAccount")]
        public async Task<IActionResult> Register(NhanvienRegisterDTO nhanvienRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                // Dữ liệu không hợp lệ, trả về lỗi cùng với các thông báo lỗi cụ thể
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _accountSerivce.RegisterAccount(nhanvienRegisterDTO);
                if (result.Success) return Ok(result);
                else
                {
                    // Trả về mã lỗi với thông tin từ ResponseObj nếu thất bại
                    return BadRequest(new
                    {
                        Message = result.Message,
                        Errors = result.Errors
                    });
                }

            }
            catch (Exception ex)
            {
                // Trong trường hợp có lỗi ngoài mong đợi, trả về 500 hoặc một mã lỗi tùy chỉnh
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra khi đăng ký tài khoản.",
                    Error = ex.Message
                });
            }   

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginResquest loginRequest)
        {
            var user = await _context.NhanViens
       .Include(nv => nv.ChucVu) // Đảm bảo lấy thông tin chức vụ
       .FirstOrDefaultAsync(u => u.TaiKhoan == loginRequest.TaiKhoan && u.MatKhau == loginRequest.MatKhau);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _accountSerivce.LoginAccount(loginRequest);
                Console.WriteLine($"API Login Response: {JsonConvert.SerializeObject(result)}");


                if (result.Success)
                {
                    return Ok(result);
                }

                // Trả về Unauthorized nếu đăng nhập thất bại
                return Unauthorized(new
                {
                    Message = "Tài khoản hoặc mật khẩu không chính xác",
                    ErrorDetails = result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Đã xảy ra lỗi khi xử lý đăng nhập.",
                    Error = ex.Message
                });
            }
        }
    }
}
