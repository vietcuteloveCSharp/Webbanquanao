using DTO.VuvietanhDTO.NhanViens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.NTTuyenServices.IServices;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly INhanVienServices _nhanVienService;
        public NhanVienController(INhanVienServices nhanVienService, IConfiguration configuration)
        {
            this._nhanVienService = nhanVienService;
            this._configuration = configuration;
        }
        [HttpGet("Get_NhanVien_ByID")]
        public async Task<IActionResult> GetNhanVienById(int id) 
        {
            try
            {
                NhanVienProfileDTO result =await _nhanVienService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

               return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
