﻿//using DTO.NTTuyen.HoaDons;
using Microsoft.AspNetCore.Mvc;
using Service.NTTuyenServices.IServices;
using Service.VuVietAnhService.IRepository.IHoadon;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDon_NTTController : Controller
    {
        private readonly IHoadonService _hoaDonService;
        private IConfiguration configuration;
        public HoaDon_NTTController(IHoadonService _hoaDonService, IConfiguration _configuration)
        {
            _hoaDonService = _hoaDonService;
            configuration = _configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _hoaDonService.GetAllHoaDon();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra khi lấy dữ liệu",
                    Error = ex.Message
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _hoaDonService.GetHoaDonById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra khi lấy dữ liệu",
                    Error = ex.Message
                });
            }
        }
       
    }
}
