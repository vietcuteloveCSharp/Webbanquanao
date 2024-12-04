using DTO.VuvietanhDTO.Chucvus;
using DTO.VuvietanhDTO.Cuahangs;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.IChucvu;
using Service.VuVietAnhService.IRepository.ICuahang;


namespace WebAPI.Controllers
{   //hoàn thiện CRUD API cửa hàng
    [ApiController]
    [Route("api/[controller]")]
    public class CuaHangController : ControllerBase
    {
        private readonly ICuahangService _cuahangService;
        private readonly IConfiguration _configuration;
        public CuaHangController(ICuahangService cuahangService, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._cuahangService = cuahangService;
        }
        [HttpGet("Get-All-Cuahang")]
        public async Task<ActionResult<IEnumerable<FullCuahangDTO>>> GetAllCuahang()
        {
            try
            {
                var result = await _cuahangService.GetAllCuaHang();
                if(!result.Any())
                    return NotFound(new { Message = "Không có dữ liệu cửa hàng." });
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpPost("Add-Cuahang")]
        public async Task<IActionResult> AddCuahang([FromBody] CuahangDTO cuahangDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu cửa hàng không hợp lệ.");
            }

            try
            {
                var result = await _cuahangService.AddCuahang(cuahangDTO);
                return CreatedAtAction(nameof(AddCuahang), new { id = result.Ten }, new
                {
                    message = "Thêm cửa hàng thành công!",
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
        public async Task<IActionResult> GetCuaHangById(int id)
        {
            try
            {
                var cuaHang = await _cuahangService.GetCuaHangById(id);
                return Ok(cuaHang);

            }
            catch(KeyNotFoundException ex)
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
        public async Task<IActionResult> DeleteCuaHang(int id)
        {
            try
            {
                var message = await _cuahangService.DeleteCuahang(id);
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
        public async Task<IActionResult> UpdateCuahang(int id, [FromBody] CuahangDTO updatedCuahangDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            try
            {
                var updatedCuahang = await _cuahangService.UpdateCuahang(id, updatedCuahangDTO);
                return Ok(new
                {
                    message = "Cập nhật cửa hàng thành công!",
                    updatedCuaHang = updatedCuahang
                });


            }
            catch (KeyNotFoundException ex)
            {
                // Lỗi logic liên quan đến dữ liệu
                return NotFound(new { Message = ex.Message });
            }
            catch(InvalidOperationException ex)
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
