using DTO.VuvietanhDTO.Mausacs;
using DTO.VuvietanhDTO.Thuonghieus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.IThuonghieu;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuonghieuController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IThuonghieuSerivce _thuonghieuSerivce;
        public ThuonghieuController(IThuonghieuSerivce thuonghieuSerivce)
        {
            this._thuonghieuSerivce= thuonghieuSerivce;
        }
        [HttpGet("Get-All-ThuongHieu")]
        public async Task<ActionResult<IEnumerable<FullThuongHieuDTO>>> GetAllThuongHieu()
        {
            try
            {
                var result = await _thuonghieuSerivce.GetAllThuongHieu();
                if (!result.Any())
                    return NotFound(new { Message = "Không có dữ liệu màu sắc" });
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpPost("Add-ThuongHieu")]
        public async Task<IActionResult> AddThuongHieu([FromBody] CreateThuongHieuDTO createThuongHieuDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _thuonghieuSerivce.AddThuongHieu(createThuongHieuDTO);
                return CreatedAtAction(nameof(AddThuongHieu), new
                {
                    message = "Thêm thương hiệu thành công",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetThuongHieuById(int id)
        {
            try
            {
                var thuongHieu = await _thuonghieuSerivce.GetThuongHieuById(id);
                return Ok(thuongHieu);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteThuongHieu(int id)
        {
            try
            {
                var message = await _thuonghieuSerivce.DeleteThuongHieu(id);
                return Ok(new { Message = message });
            }
            catch (KeyNotFoundException ex)
            {
                // Lỗi logic liên quan đến dữ liệu
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi hệ thống
                return StatusCode(500, new { Message = "Đã xảy ra lỗi không mong đợi.", Details = ex.Message });
            }
        }
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateThuongHieu(int id, [FromBody] ThuongHieuDTO thuongHieuDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedMausac = await _thuonghieuSerivce.UpdateThuongHieu(id, thuongHieuDTO);
                return Ok(new
                {
                    message = "Cập nhật màu sắc thành công!",
                    updateThuongHieu = thuongHieuDTO
                });


            }
            catch (KeyNotFoundException ex)
            {
                // Lỗi logic liên quan đến dữ liệu
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi hệ thống
                return StatusCode(500, new { Message = "Đã xảy ra lỗi không mong đợi.", Details = ex.Message });
            }
        }
        
    }
}
