using DTO.VuvietanhDTO.Kichthuocs;
using DTO.VuvietanhDTO.Mausacs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.IKichthuoc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KichthuocController : ControllerBase
    {
        private readonly IKichthuocService _kichthuocService;
        public KichthuocController(IKichthuocService kichthuocService) 
        {
            this._kichthuocService = kichthuocService;
        }
        [HttpGet("Get-All-Kichthuoc")]
        public async Task<ActionResult<IEnumerable<FullKichThuocDTO>>> GetAllKichThuoc()
        {
            try
            {
                var result = await _kichthuocService.GetAllKichThuoc();
                if (!result.Any())
                    return NotFound(new { Message = "Không có dữ liệu kích thước" });
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpPost("Add-KichThuoc")]
        public async Task<IActionResult> AddKichThuoc([FromBody] KichThuocDTO kichThuocDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _kichthuocService.AddKichThuoc(kichThuocDTO);
                return CreatedAtAction(nameof(AddKichThuoc), new { id = result.Ten }, new
                {
                    message = "Thêm kích thước thành công",
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
        public async Task<IActionResult> GetKichThuocById(int id)
        {
            try
            {
                var kichThuoc = await _kichthuocService.GetKickThuocId(id);
                return Ok(kichThuoc);

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
        //[HttpDelete("delete/{id:int}")]
        //public async Task<IActionResult> DeleteSanPham(int id)
        //{
        //    try
        //    {
        //        var message = await _mausacService.DeleteMauSac(id);
        //        return Ok(new { Message = message });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        // Lỗi logic liên quan đến dữ liệu
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Lỗi hệ thống
        //        return StatusCode(500, new { Message = "Đã xảy ra lỗi không mong đợi.", Details = ex.Message });
        //    }
        //}
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateKichThuoc(int id, [FromBody] KichThuocDTO kichThuocDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedKichThuoc= await _kichthuocService.UpdateKichThuoc(id, kichThuocDTO);
                return Ok(new
                {
                    message = "Cập nhật kích thước thành công!",
                    updatedKichThuoc = kichThuocDTO
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
