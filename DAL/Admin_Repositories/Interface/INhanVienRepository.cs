using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface INhanVienRepository
    {
        List<NhanVien> GetAll();
        NhanVien GetById(int id);   
        bool Add(NhanVien obj);    
        bool Update( NhanVien obj);
        bool Delete(int id);
    }
}
