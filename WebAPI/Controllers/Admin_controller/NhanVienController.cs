using DTO.TuyenNT;
using Microsoft.AspNetCore.Mvc;
using Service.IServices_Admin;



namespace WebAPI.Controllers.Admin_controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienServices _service;
        private readonly IConfiguration _configuration;

        public NhanVienController(INhanVienServices service, IConfiguration configuration)
        {
            this._service = service;
            this._configuration = configuration;
        }

        [HttpGet("getAllNhanVien")]
        public async Task<ActionResult<IEnumerable<NhanVienDTO>>> GetAll()
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
                var nhanvien = await _service.GetById(id);
                return Ok(nhanvien);
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
        [HttpPost("addNhanVien")]
        public async Task<IActionResult> Add([FromBody] NhanVienDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
            try
            {
                var result = await _service.Add(dto);
                return CreatedAtAction(nameof(Add), new { id = result.TenNhanVien, chucvu = result.Id_ChucVu }, new { message = "Thêm nhân viên thành công ", data = result });
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
        public async Task<IActionResult> Update(int id, [FromBody] NhanVienDTO dto)
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
