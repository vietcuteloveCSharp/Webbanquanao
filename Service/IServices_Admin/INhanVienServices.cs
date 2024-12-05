using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface INhanVienServices
    {
        Task<List<NhanVienDTO>> GetAll();
        Task<NhanVienDTO> GetById(int id);
        Task<NhanVienDTO> Add(NhanVienDTO obj);
        Task<NhanVienDTO> Update(int id,NhanVienDTO obj);
        Task<NhanVienDTO> Delete(int id);
    }
}
