using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IGioHangRepository
    {
        List<GioHang> GetAll(); 
        GioHang GetById(int id);
        bool Add(GioHang obj);
        bool Update(GioHang obj);
        bool Delete(GioHang obj);
    }
}
