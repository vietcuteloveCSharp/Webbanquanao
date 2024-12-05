using DTO.TuyenNT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IServices_Admin;

namespace WebAPI.Controllers.Admin_controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuongHieuController : ControllerBase
    {
        private readonly IThuongHieuService _service;
        private readonly IConfiguration _configuration;

        public ThuongHieuController(IThuongHieuService service, IConfiguration configuration)
        {
            this._service = service;
            this._configuration = configuration;
        }

        [HttpGet("getAllThuongHieu")]
        public async Task<ActionResult<IEnumerable<ThuongHieuDTO>>> GetAll()
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
                var result = await _service.GetById(id);
                return Ok(result);
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
        [HttpPost("addThuongHieu")]
        public async Task<IActionResult> Add([FromBody] ThuongHieuDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
            try
            {
                var result = await _service.Add(dto);
                return CreatedAtAction(nameof(Add), new { id = result.Ten }, new { message = "Thêm nhân viên thành công ", data = result });
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
        public async Task<IActionResult> Update(int id, [FromBody] ThuongHieuDTO dto)
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
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }
        }
    }
}
