using DAL.Entities;
using DTO.VuvietanhDTO.Calamviecs;
using DTO.VuvietanhDTO.Ngaylamviecs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.ICalamviec;
using Service.VuVietAnhService.IRepository.INgaylamviec;
using Service.VuVietAnhService.Repository.Calamviec;
using Service.VuVietAnhService.Repository.Ngaylamviec;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NgaylamViecController : ControllerBase
    {
        private readonly INgaylamviecService _ngaylamviecService;
        private readonly IConfiguration _configuration;
        public NgaylamViecController(INgaylamviecService ngaylamviecService, IConfiguration configuration)
        {
            this._ngaylamviecService = ngaylamviecService;
            this._configuration = configuration;
        }
        //lấy list ngay lam viec
        [HttpGet("Get-All-NgayLamViec")]
        public async Task<ActionResult<IEnumerable<NgayLamViecDTO>>> GetAllNgayLamViec()
        {
            try
            {
                var result = await _ngaylamviecService.GetAllNgayLamViec();
                if (!result.Any())
                    return NotFound(new { Message = "Không có dữ liệu ngày làm việc" });
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpGet("Get-CaLamViecByNgayLamViecId/{ngayLamViecId:int}")]
        public async Task<IActionResult> GetCaLamViecByNgayLamViecId(int ngayLamViecId)
        {
            try
            {
                var caLamViec = await _ngaylamviecService.GetCaLamViecByNgayLamViecIdAsync(ngayLamViecId);
                return Ok(caLamViec);
            }
            catch (KeyNotFoundException ex)
            {
                // Bắt exception khi không tìm thấy ngày làm việc và trả về NotFound
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNgayLamViecId(int id)
        {
            try
            {
                var ngayLamViec = await _ngaylamviecService.GetNgayLamViecById(id);
                return Ok(ngayLamViec);
            }
            catch (KeyNotFoundException ex)
            {
                // Bắt exception khi không tìm thấy ngày làm việc và trả về NotFound
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }

        }
        [HttpPost("Add-NgayLamViec")]
        public async Task<IActionResult> CreateNgayLamViec([FromBody]CreateNgayLamViecDTO createNgayLamViecDTO)
        {

            ArgumentNullException.ThrowIfNull(createNgayLamViecDTO);
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu ngày làm việc không hợp lệ.");
            }
            try
            {
                var result = await _ngaylamviecService.CreateNgayLamviec(createNgayLamViecDTO);
                return CreatedAtAction(nameof(CreateNgayLamViec), new
                {
                    message = "Thêm ngày làm việc thành công!"
                });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }

        }
        //[HttpDelete("delete/{id}")]
        //public async Task<IActionResult> DeleteNgayLamViec(int id)
        //{
        //    try
        //    {
        //        bool result = await _ngaylamviecService.DeleteNgayLamViec(id);
        //        if (result)
        //            return Ok("Xóa ngày làm việc thành công!");
        //        return BadRequest("Xóa ngày làm việc thất bại!");
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Lỗi hệ thống: " + ex.Message);
        //    }
        //}
    }

}
