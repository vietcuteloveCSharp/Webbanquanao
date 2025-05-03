using DTO.VuvietanhDTO.Canhanviens;
using Microsoft.AspNetCore.Mvc;
using Service.VuVietAnhService.IRepository.ICanhanvien;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaNhanVienController : ControllerBase
    { private readonly ICanhanvienService _canhanvienService;

        public CaNhanVienController(ICanhanvienService canhanvienService)
        {
            this._canhanvienService = canhanvienService;
        }
        [HttpPost("add-calamviec")]
        public async Task<IActionResult> AddNhanVienToCaLamViec(int idNhanVien, int idCaLamViec)
        {
            var result = await _canhanvienService.CreatCanhanvien(idNhanVien, idCaLamViec);
            return result ? Ok("Thêm thành công") : BadRequest("Nhân viên đã có trong ca này!");
        }
        [HttpGet("get-nhanviens-by-ca/{idCaLamViec}")]
        public async Task<IActionResult> GetNhanViensByCa(int idCaLamViec)
        {
            var result = await _canhanvienService.GetNhanVienByCa(idCaLamViec);
            if (result == null) return BadRequest("Nhân viên không có ca này");
            return Ok(result);
        }
        [HttpGet("get-ca-by-nhanvien")]
        public async Task<IActionResult> GetCaLamViecsByNhanVien(int idNhanVien)
        {
            var result = await _canhanvienService.GetCaLamViecByNhanVien(idNhanVien);
            if (result == null) return BadRequest("không có dữ liệu ca làm việc của nhân viên này");
            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateNhanVienCaLamViec(DoiCaDTO doiCaDTO)
        {
            var result = await _canhanvienService.UpdateCanhanvien(doiCaDTO);
            return result ? Ok("Đổi ca thành công") : BadRequest("Không thể đổi ca");
        }
    }
}
