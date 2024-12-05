using DTO.TuyenNT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IServices_Admin;

namespace WebAPI.Controllers.Admin_controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _service;
        private readonly IConfiguration _configuration;

        public HoaDonController(IHoaDonService service, IConfiguration configuration)
        {
            this._service = service;
            this._configuration = configuration;
        }

        [HttpGet("getAllHoaDon")]
        public async Task<ActionResult<IEnumerable<HoaDonDTO>>> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                if (!result.Any())
                {
                    return NotFound("Không có dữ liệu");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var hoadon = await _service.GetById(id);
                return Ok(hoadon);
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi :", Details = ex.Message });
            }
        }
        [HttpPost("addHoaDon")]
        public async Task<IActionResult> Add([FromBody] HoaDonDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
            try
            {
                var result = await _service.Add(dto);
                return CreatedAtAction(nameof(Add), new {id = result.NgayTao , createdate=result.Id_KhachHang}, new { message = "Thêm danh mục thành công ", data = result });
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }
        }
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HoaDonDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
            try
            {
                var result = await _service.Update(id, dto);
                return Ok(new { Message = "Cập nhật thành công", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }
        }
        [HttpPut("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
            try
            {
                var result = await _service.Delete(id);
                return Ok(new { Message = "Cập nhật thành công", Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }
        }
    }
}
