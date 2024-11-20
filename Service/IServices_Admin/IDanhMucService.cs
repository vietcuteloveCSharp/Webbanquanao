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
        string Add(DanhMucDTO obj);
        string Delete(int id);
        string Update(DanhMucDTO obj);
        List<DanhMucDTO> GetAll();
        DanhMucDTO GetById(int id);
    }
}
