using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface INhanVienServices
    {
        List<NhanVien> GetAll();
        NhanVien GetById(int id);
        string  Add(NhanVien obj);
        string Update(NhanVien obj);    
        string Delete(int id);
    }
}
