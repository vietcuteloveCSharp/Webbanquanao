using DTO.NTTuyen.ChiTietHoaDon;
using Microsoft.AspNetCore.Mvc;
using Service.NTTuyenServices.IServices;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietHoaDonController : Controller
    {
        private readonly IChiTietHoaDonService _chiTietHoaDonService;
        private readonly IConfiguration _configuration;
       public ChiTietHoaDonController(IChiTietHoaDonService chiTietHoaDonService, IConfiguration configuration)
        {
            _chiTietHoaDonService = chiTietHoaDonService;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _chiTietHoaDonService.GetAll();
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
                var result = await _chiTietHoaDonService.GetById(id);
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
        public async Task<IActionResult> Add(ChiTietHoaDonDTO chiTietHoaDonDTO)
        {
            try
            {
                var result = await _chiTietHoaDonService.Add(chiTietHoaDonDTO);
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
        [HttpPost]
        public async Task<IActionResult> Update(int id ,ChiTietHoaDonDTO chiTietHoaDonDTO)
        {
            try
            {
                var result = await _chiTietHoaDonService.Update(id,chiTietHoaDonDTO);
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
