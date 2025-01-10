using Enum.EnumVVA;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.IHoadon;
using Service.VuVietAnhService.Repository.Hoadon;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoadonController : ControllerBase
    {
        private readonly IHoadonService _hoaDonService;
        public HoadonController(IHoadonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }
        /// <summary>
        /// Lấy một hoá đơn theo Id
        /// </summary>
        /// <param name="id">ID hoá đơn</param>
        /// <returns>Kết quả của một hoá đơn</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHoaDonById(int id)
        {
            try
            {
                var result = await _hoaDonService.GetHoaDonById(id);
                return Ok(result);

            }
            catch (KeyNotFoundException ex)
            {
                // Trả về lỗi nếu không tìm thấy hóa đơn
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Trả về lỗi nếu trạng thái không hợp lệ
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Trả về lỗi không dự kiến
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra trong quá trình xử lý.",
                    Error = ex.Message
                });
            }
        }
        /// <summary>
        /// Cập nhật trạng thái hóa đơn.
        /// </summary>
        /// <param name="id">ID hóa đơn</param>
        /// <param name="nextTrangThai">Trạng thái mới</param>
        /// <returns>Kết quả cập nhật</returns>
        [HttpPut("{id}/update-trangthai-hoadon")]
        public async Task<IActionResult> UpdateTrangThaiHoaDon(int id, ETrangThaiHD nextTrangThai)
        {
            try
            {
                // Gọi service để cập nhật trạng thái
                var result = await _hoaDonService.UpdateTrangThai(id, nextTrangThai);

                // Trả về kết quả thành công
                return Ok(new
                {
                    Message = "Cập nhật trạng thái hóa đơn thành công.",
                    Data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                // Trả về lỗi nếu không tìm thấy hóa đơn
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Trả về lỗi nếu trạng thái không hợp lệ
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Trả về lỗi không dự kiến
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra trong quá trình xử lý.",
                    Error = ex.Message
                });
            }
        }
    }
}

