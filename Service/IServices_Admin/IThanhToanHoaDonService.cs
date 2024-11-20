using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IThanhToanHoaDonService
    {
        Task<List<ThanhToanHoaDonDTO>> GetAll();
        Task<ThanhToanHoaDonDTO> GetById(int id);
        Task<ThanhToanHoaDonDTO> Add(ThanhToanHoaDonDTO obj);
        Task<ThanhToanHoaDonDTO> Update(int id,ThanhToanHoaDonDTO obj);

    }
}
