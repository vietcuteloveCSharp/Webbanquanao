using AutoMapper.Configuration.Annotations;
using DAL.Entities;
using DTO.TuyenNT;
using Microsoft.AspNetCore.Mvc;
using Service.IServices_Admin;

namespace WebAPI.Controllers.Admin_controller
{
    public class DanhMucController : ControllerBase
    {
        private readonly IDanhMucService _service;
        private readonly IConfiguration _configuration;
        public DanhMucController(IDanhMucService service, IConfiguration configuration)
        {
            this._service = service;
            this._configuration = configuration;
        }
        [HttpGet("getAllDanhMuc")]
        public async Task<ActionResult<IEnumerable<DanhMucDTO>>> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
                if (!result.Any())
                {
                    return NotFound("Không có dữ liệu Danh mục");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var danhmuc = await _service.GetById(id);
                return Ok(danhmuc);
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi :", Details = ex.Message });
            }
        }
        [HttpPost("addDanhMuc")]
        public async Task<IActionResult> Add([FromBody] DanhMucDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
            try
            {
                var result = await _service.Add(dto);
                return CreatedAtAction(nameof(Add), new { id = result.TenDanhMuc }, new { message = "Thêm danh mục thành công ", data = result });
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }
        }
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DanhMucDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
            try
            {
                var result = await _service.Update(id,dto);
                return Ok(new { Message = "Cập nhật thành công",Data = result });
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
                return StatusCode(500, new { Message = "Lỗi", Details = ex.Message });
            }
        }
    }
}
