using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IHoaDonService
    {
        Task<List<HoaDonDTO>> GetAll();
        Task<HoaDonDTO> GetById(int id);
        Task<HoaDonDTO> Add(HoaDonDTO obj);
        Task<HoaDonDTO> Update(int id, HoaDonDTO obj);
        Task<HoaDonDTO> Delete(int id);
    }
}
