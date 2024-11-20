using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.IServices_Admin;

namespace WebAPI.Controllers.Admin_controller
{
    public class DanhMucController : ControllerBase
    {
        private readonly IDanhMucService _service;

        public DanhMucController(IDanhMucService service)
        {
            _service = service;
        }
        
           
    }
}
