using DTO.VuvietanhDTO.Danhmucs;
using DTO.VuvietanhDTO.Mausacs;
using DTO.VuvietanhDTO.Sanphams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.IMausac;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MausacController : ControllerBase
    {
        private readonly IMausacService _mausacService;

        public MausacController(IMausacService mausacService)
        {
            _mausacService = mausacService;
        }
        [HttpGet("Get-All-MauSac")]
        public async Task<ActionResult<IEnumerable<MauSacDTO>>> GetAllMauSac()
        {
            try
            {
                var result = await _mausacService.GetAllMausac();
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
        [HttpPost("Add-Mausac")]
        public async Task<IActionResult> AddMausac([FromBody] MauSacDTO mauSacDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mausacService.AddMauSac(mauSacDTO);
                return CreatedAtAction(nameof(AddMausac), new { id = result.Ten }, new
                {
                    message = "Thêm màu sắc thành công",
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
        public async Task<IActionResult> GetMauSacById(int id)
        {
            try
            {
                var mauSac = await _mausacService.GetMauSacById(id);
                return Ok(mauSac);

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
        public async Task<IActionResult> DeleteSanPham(int id)
        {
            try
            {
                var message = await _mausacService.DeleteMauSac(id);
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
        public async Task<IActionResult> UpdateMauSac(int id, [FromBody] MauSacDTO mauSacDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedMausac = await _mausacService.UpdateMauSac(id, mauSacDTO);
                return Ok(new
                {
                    message = "Cập nhật màu sắc thành công!",
                    updateMauSac = mauSacDTO
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
