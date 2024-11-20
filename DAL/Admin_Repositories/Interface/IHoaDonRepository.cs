using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IHoaDonRepository
    {
        List<HoaDon> GetAll();
        HoaDon GetById(int id);
        bool Add(HoaDon obj);    
        bool Update(HoaDon obj);
        bool Delete(int id);
    }
}
