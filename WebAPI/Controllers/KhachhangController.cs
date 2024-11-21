using DTO.Cuahangs;
using DTO.VuvietanhDTO.KhachHangs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Reponses.Resquest;
using Service.VuVietAnhService.IRepository.IAccountKhachHang;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class KhachhangController :ControllerBase
    {
        private readonly IAccountKHService _accountKHService;
        private readonly IConfiguration _configuration;
        public KhachhangController(IAccountKHService accountKHService, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._accountKHService = accountKHService;
        }
       
        [HttpPost("RegisterClient")]
        public async Task<IActionResult> RegisterKH(KhachhangDTO khachhangDTO)
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState); //data không hợp lệ trả về lỗi tương ứng
            }
            else
            {
                var taikhoanKH = await _accountKHService.CheckTaikhoanExitst(khachhangDTO.TaiKhoan);
                if(taikhoanKH){
                    try
                    {
                        await _accountKHService.RegisterAccountKH(khachhangDTO);
                        return Ok(new { message = "Đăng kí tài khoản thành công"});
                    }
                    catch (Exception)
                    {
                        return StatusCode(400, "Có lỗi xảy ra khi đăng ký tài khoản.");
                    }
                }
                else
                {
                    return BadRequest(new { message = "tài khoản đã tồn tại" });
                }
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
                var result = await _accountKHService.LoginKH(loginResquest);
                if (result.Success) return Ok(result);
                return Unauthorized();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("Get-All-AccountKH")]
        public async Task<ActionResult<IEnumerable<KhachhangDTO>>> GetAllAccountKH()
        {
            var result = await _accountKHService.GetAllAccountKH();
            if (result == null && !result.Any())
                return Ok(new { Message = "Không có tài khoản nào.", Data = "null" });
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccountKHById(int id)
        {
            var cuaHang = await _accountKHService.GetAccountKHById(id);
            if (cuaHang == null)
            {
                return NotFound(new { Message = "Không tìm thấy tài khoản với ID này." });
            }
            return Ok(cuaHang);
        }
    }
}
