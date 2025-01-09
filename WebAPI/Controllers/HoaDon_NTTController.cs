using DTO.NTTuyen.HoaDons;
using Microsoft.AspNetCore.Mvc;
using Service.NTTuyenServices.IServices;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDon_NTTController : Controller
    {
        private readonly IHoaDon2Service _hoaDonService;
        private IConfiguration configuration;
        public HoaDon_NTTController(IHoaDon2Service hoaDonService, IConfiguration _configuration)
        {
            _hoaDonService = hoaDonService;
            configuration = _configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _hoaDonService.GetAll();
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _hoaDonService.GetById(id);
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
        [HttpPost]
        public async Task<IActionResult> Add(HoaDonDTO hoaDonDTO)
        {
            try
            {
                var result = await _hoaDonService.Add(hoaDonDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra khi thêm dữ liệu",
                    Error = ex.Message
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, HoaDonDTO hoaDonDTO)
        {
            try
            {
                var result = await _hoaDonService.Update(id, hoaDonDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra khi cập nhật dữ liệu",
                    Error = ex.Message
                });
            }
        }
    }
}
