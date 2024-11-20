using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IPhuongThucThanhToanService
    {
        Task<List<PhuongThucThanhToanDTO>> GetAll();
        Task<PhuongThucThanhToanDTO> GetById(int id);
        Task<PhuongThucThanhToanDTO> Add(PhuongThucThanhToanDTO obj);
        Task<PhuongThucThanhToanDTO> Update(int id,PhuongThucThanhToanDTO obj);
        Task<PhuongThucThanhToanDTO> Delete(int id);
    }
}
