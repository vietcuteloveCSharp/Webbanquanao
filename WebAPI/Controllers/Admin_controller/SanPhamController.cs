using DTO.TuyenNT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.IServices_Admin;

namespace WebAPI.Controllers.Admin_controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamService _service;
        private readonly IConfiguration _configuration;

        public SanPhamController(ISanPhamService service, IConfiguration configuration)
        {
            this._service = service;
            this._configuration = configuration;
        }

        [HttpGet("getAllSanPham")]
        public async Task<ActionResult<IEnumerable<SanPhamDTO>>> GetAll()
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
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", detail = ex.Message });
            }

        }
        [HttpPost("addSanPham")]
        public async Task<IActionResult> Add([FromBody] SanPhamDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ ");
            }
            try
            {
                var result = await _service.Add(dto);
                return CreatedAtAction(nameof(Add), new { id = result.Ten }, new { Message = "Thêm thành công", data = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Messsage = "Lỗi", Detail = ex.Message });
            }
        }
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SanPhamDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");

            }
            try
            {
                var result = await _service.Update(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", detail = ex.Message });
            }
        }
        [HttpPut("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var result = await _service.Delete(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", detail = ex.Message });
            }
        }
    }
}
