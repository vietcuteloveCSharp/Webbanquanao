using DAL.Entities;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.NhanViens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.IAuthentication
{
    public interface IAuthenService
    {
        //tạo token
        Task<string> GenerateJwtToken(NhanvienDTO nhanvienDTO);
        Task<string> GenerateJwtToken(KhachhangDTO khachhangDTO);

    }
}
