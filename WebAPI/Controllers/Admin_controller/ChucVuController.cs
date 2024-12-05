using DTO.TuyenNT;
using Microsoft.AspNetCore.Mvc;
using Service.IServices_Admin;
using Service.Services_Admin;


namespace WebAPI.Controllers.Admin_controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucVuController : ControllerBase
    {
        private readonly IChucVuService _chucVuService;
        private readonly IConfiguration _configuration;

        public ChucVuController(IChucVuService chucVuService, IConfiguration configuration)
        {
            this._chucVuService = chucVuService;
            this._configuration = configuration;
        }

        // GET: api/<ChucVuController>
        [HttpGet("GetAllChucVu")]
        public async Task<ActionResult<IEnumerable<ChucVuDTO>>> GetAllChucVu()
        {
             try
            {
                var result = await _chucVuService.GetAll();
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

        // GET api/<ChucVuController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ChucVuController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ChucVuController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChucVuController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
