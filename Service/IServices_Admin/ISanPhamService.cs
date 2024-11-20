using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface ISanPhamService
    {
        List<SanPham> GetAll();
        SanPham GetById(int id);
        string Add(SanPham obj);
        string Update(SanPham obj);
        string Delete(int id);
    }
}
    