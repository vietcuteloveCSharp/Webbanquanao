using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IChucVuService
    {
        Task<List<ChucVuDTO>> GetAll();
        Task<ChucVuDTO> GetById(int id);
        Task<ChucVuDTO> Add(ChucVuDTO obj);
        Task<ChucVuDTO> Update(int id,ChucVuDTO obj);
       
    }
}
