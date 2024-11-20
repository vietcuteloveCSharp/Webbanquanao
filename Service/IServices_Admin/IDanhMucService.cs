using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IDanhMucService
    {
        Task<DanhMucDTO> Add(DanhMucDTO obj);
        Task<DanhMucDTO> Delete(int id);
        Task<DanhMucDTO> Update(int id,DanhMucDTO obj);
        Task<List<DanhMucDTO>> GetAll();
        Task<DanhMucDTO> GetById(int id);
    }
}
