using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Danhmucs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.IDanhmuc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhmucController : ControllerBase
    {
        private readonly IDanhmucService _danhmucService;

        public DanhmucController(IDanhmucService danhmucService)
        {
            this._danhmucService = danhmucService;
        }
            [HttpGet("Get-All-Sanpham")]
            public async Task<ActionResult<IEnumerable<DanhMucDTO>>> GetAllDanhMuc()
            {
                try
                {
                    var result = await _danhmucService.GetAllDanhMuc();
                    if (!result.Any())
                        return NotFound(new { Message = "Không có dữ liệu danh mục." });
                    return Ok(result);

                }
                catch (Exception ex)
                {
                    // Bắt tất cả các exception khác và trả về InternalServerError
                    return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Details = ex.Message });
                }
            }
            [HttpPost("Create-DanhMuc")]
            public async Task<IActionResult> CreateDanhMuc([FromBody] CreatDanhMucDTO creatDanhMucDTO)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var result = await _danhmucService.CreateDanhMuc(creatDanhMucDTO);
                    return CreatedAtAction(nameof(CreateDanhMuc), new { id = result.TenDanhMuc }, new
                    {
                        message = "Thêm danh mục thành công!",
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
            public async Task<IActionResult> GetDanhMucById(int id)
            {
                try
                {
                    var danhMuc = await _danhmucService.GetDanhMucById(id);
                    return Ok(danhMuc);

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
            //public async Task<IActionResult> DeleteCuaHang(int id)
            //{
            //    try
            //    {
            //        var message = await _cuahangService.DeleteCuahang(id);
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
            public async Task<IActionResult> UpdateDanhMuc(int id, [FromBody] UpdateDanhMucDTO updateDanhMucDTO)
            {
                if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            try
            {
                var updatedCuahang = await _danhmucService.UpdateDanhMuc(id, updateDanhMucDTO);
                return Ok(new
                {
                    message = "Cập nhật danh mục thành công!",
                    updatedDanhmuc = updateDanhMucDTO
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
