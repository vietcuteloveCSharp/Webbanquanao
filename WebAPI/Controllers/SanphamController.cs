using DTO.VuvietanhDTO.Danhmucs;
using DTO.VuvietanhDTO.Sanphams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.ISanpham;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanphamController : ControllerBase
    {
        private readonly ISanphamSerivce _sanphamSerivce;
        public SanphamController(ISanphamSerivce sanphamSerivce)
        {
            this._sanphamSerivce = sanphamSerivce;
        }
        [HttpGet("Get-All-SanPham")]
        public async Task<ActionResult<IEnumerable<DanhMucDTO>>> GetAllSanPham()
        {
            try
            {
                var result = await _sanphamSerivce.GetAllSanPham();
                if (!result.Any())
                    return NotFound(new { Message = "Không có dữ liệu sản phẩm" });
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpPost("Create-SanPham")]
        public async Task<IActionResult> CreateSanPham([FromBody] CreatSanPhamDTO creatSanPhamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _sanphamSerivce.CreateSanPham(creatSanPhamDTO);
                return CreatedAtAction(nameof(CreateSanPham), new { id = result.Ten }, new
                {
                    message = "Thêm sản phẩm thành công",
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
        public async Task<IActionResult> GetSanPhamById(int id)
        {
            try
            {
                var sanPham= await _sanphamSerivce.GetSanPhamById(id);
                return Ok(sanPham);

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
        public async Task<IActionResult> DeleteSanPham (int id)
        {
            try
            {
                var message = await _sanphamSerivce.DeleteSanPham(id);
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
        public async Task<IActionResult> UpdateDanhMuc(int id, [FromBody] UpdateSanPhamDTO updateSanPhamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedSanPham = await _sanphamSerivce.UpdateSanPham(id, updateSanPhamDTO);
                return Ok(new
                {
                    message = "Cập nhật sản phẩm thành công!",
                    updateSanPham = updateSanPhamDTO
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
