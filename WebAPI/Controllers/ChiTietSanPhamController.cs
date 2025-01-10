using Microsoft.AspNetCore.Mvc;
using Service.NTTuyenServices.IServices;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietSanPhamController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IChiTietSanPhamServices _services;

        public ChiTietSanPhamController(IConfiguration configuration, IChiTietSanPhamServices services)
        {
            _configuration = configuration;
            _services = services;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _services.GetAllChiTietSanPham();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(  500,new
                { 
                    Message = "Có lỗi xảy ra khi lấy dữ liệu",
                    Error = ex.Message
                });
            }
        }
        [HttpGet("{id}")]   
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _services.GetChiTietSanPhamById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra khi lấy dữ liệu",
                    Error = ex.Message
                });
            }
        }
    }
}
