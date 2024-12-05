using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface ISanPhamService
    {
        Task<List<SanPhamDTO>> GetAll();
        Task<SanPhamDTO> GetById(int id);
        Task<SanPhamDTO> Add(SanPhamDTO obj);
        Task<SanPhamDTO> Update(int id, SanPhamDTO obj);
        Task<SanPhamDTO> Delete(int id);
    }
}
    