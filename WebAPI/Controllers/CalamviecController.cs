using Azure.Core;
using DTO.VuvietanhDTO.Calamviecs;
using DTO.VuvietanhDTO.Chucvus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.ICalamviec;
using Service.VuVietAnhService.IRepository.IChucvu;
using Service.VuVietAnhService.Repository.Calamviec;
using Service.VuVietAnhService.Repository.Chucvu;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalamviecController :ControllerBase
    {
        private readonly ICalamviecService  _calamviecService;
        private readonly IConfiguration _configuration;
        public CalamviecController(ICalamviecService calamviecService, IConfiguration configuration)
        {
            this._calamviecService = calamviecService;
           this._configuration = configuration;
        }
        //lấy list  ca lam viec
        [HttpGet("Get-All-CaLamViec")]
        public async Task<ActionResult<IEnumerable<FullCaLamViecDTO>>> GetAllCaLamViec()
        {
            try
            {
                var result = await _calamviecService.GetAllCaLamViec();
                if (!result.Any())
                    return NotFound(new { Message = "Không có dữ liệu ca làm việc" });
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }
        //thêm ca làm việc
        [HttpPost("Add-Calamviec")]
        public async Task<IActionResult> AddCaLamViec([FromBody] CreateCaLamViecDTO createCaLamViecDTO)
        {
            ArgumentNullException.ThrowIfNull(createCaLamViecDTO);
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu ca việc làm không hợp lệ.");
            }

            try
            {
                var result = await _calamviecService.CreateCaLamViec(createCaLamViecDTO);
                return CreatedAtAction(nameof(AddCaLamViec), new
                {
                    message = "Thêm ca làm việc thành công!"                    
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = $"Dữ liệu không hợp lệ: {ex.Message}" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi cập nhật database!", Error = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                // Bắt tất cả các exception khác và trả về InternalServerError
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
            }
        }

        //lấy ca làm việc theo id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCaLamViecById(int id)
        {
            try
            {
                var caLamViec = await _calamviecService.GetCaLamViecById(id);
                return Ok(caLamViec);
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
        //get ca lam viec theo ngay
        [HttpGet("Get_By_IdNgayLamViec/{id_ngaylamviec:int}")]
        public async Task<IActionResult> GetCaLamViecByIdNgayLamViec(int id_ngaylamviec)
        {
            try
            {
                var caLamViec = await _calamviecService.GetCaLamViecByIdNgayLamViec(id_ngaylamviec);
                return Ok(caLamViec);
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
        //update ca làm việc
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateCaLamViec(int id, [FromBody] UpdateCaLamViecDTO updateCaLamViecDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu chức vụ không hợp lệ.");
            }
            try
            {
                var updatedCalamviec = await _calamviecService.UpdateCaLamViec(id, updateCaLamViecDTO);
                return Ok(new
                {
                    message = "Cập nhật chức vụ ca làm việc thành công",
                  
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
        //[HttpPut("doi-ca/{idNv1}/{idNv2}")]
        //public async Task<IActionResult> DoiCaLamViec(int idNv1, int idNv2,int idngayLamviec)
        //{
        //    try
        //    {
        //        var result = await _calamviecService.DoiCaLamViec(idNv1, idNv2,idngayLamviec);

        //        if (!result)
        //        {
        //            return BadRequest("Đổi ca thất bại, vui lòng kiểm tra lại.");
        //        }

        //        return Ok("Đổi ca thành công.");
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { Message = ex.Message });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
                
        //        return StatusCode(500, new { Message = "Có lỗi xảy ra, vui lòng thử lại sau!" });
        //    }

        //}
    }
}
