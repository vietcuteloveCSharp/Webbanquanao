using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Chucvus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.IChucvu;
using System.Numerics;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ChucvuController : ControllerBase
    {
       
        private readonly IChucvuService _chucvuService;
        private readonly IConfiguration _configuration;
        public ChucvuController(IChucvuService chucvuServic, IConfiguration configuration)
        {
            this._chucvuService = chucvuServic;
            this._configuration = configuration;
        }
        //lấy list chức vụ
        [HttpGet("Get-All-ChucVu")]
        public async Task<ActionResult<IEnumerable<ChucvuDTO>>> GetAllChucVu()
        {
            try
            {
                var result = await _chucvuService.GetAllChucVu();
                if (!result.Any())
                    return NotFound(new { Message = "Không có dữ liệu chức vụ." });
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        //lấy chức vụ theo id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetChucVuById(int id)
        {
            try
            {
                var chucVu = await _chucvuService.GetChucVuById(id);
                return Ok(chucVu);
            }
            catch (KeyNotFoundException ex)
            {
                // Bắt exception khi không tìm thấy chức vụ và trả về NotFound
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }

        }
        //thêm chức vụ
        [HttpPost("Add-ChucVu")]
        public async Task<IActionResult> AddChucVu([FromBody] ChucvuDTO chucvuDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu chức vụ không hợp lệ.");
            }

            try
            {
                var result = await _chucvuService.AddChucVu(chucvuDTO);
                return CreatedAtAction(nameof(AddChucVu), new { id = result.Ten }, new
                {
                    message = "Thêm cửa hàng thành công!",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        //update chức vụ
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateChucVu(int id, [FromBody] ChucvuDTO updatedChucvuDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu chức vụ không hợp lệ.");
            }
            try
            {
                var updatedChucvu = await _chucvuService.UpdateChucVu(id, updatedChucvuDTO);
                return Ok(new
                {
                    message = "Cập nhật chức vụ thành công",
                    updatedChucVu = updatedChucvu
                });


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
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
    }
}
