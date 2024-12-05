using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface ICuaHangRepository
    {
        List<CuaHang> GetAll();
        CuaHang GetById(int id);
        bool Add(CuaHang obj);
        bool Update(CuaHang obj);
        bool Delete(int id);
    }
}
