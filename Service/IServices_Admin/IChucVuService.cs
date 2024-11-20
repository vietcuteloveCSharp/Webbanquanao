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
        List<ChucVuDTO> GetAll();
        ChucVuDTO GetById(int id);

        string Add(ChucVuDTO obj);
        string Update(ChucVuDTO obj);
       
    }
}
