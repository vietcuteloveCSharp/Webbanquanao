using AutoMapper;
using Azure;
using DTO.VuvietanhDTO.NhanViens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Responses.Resquests;
using Service.VuVietAnhService.IRepository.IAccount;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountSerivce;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService accountSerivce, IConfiguration configuration)
        {
            _accountSerivce = accountSerivce;
            _configuration = configuration;

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
        public async Task<IActionResult> Login(LoginResquest loginResquest)
        {
            if (!ModelState.IsValid)
            {
                // Dữ liệu không hợp lệ, trả về lỗi cùng với các thông báo lỗi cụ thể
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _accountSerivce.LoginAccount(loginResquest);
               if (result.Success) return Ok(result);
               return Unauthorized();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
