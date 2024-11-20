using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IHoaDonService
    {
        List<HoaDon> GetAll();
        HoaDon GetById(int id);
        string Add(HoaDon obj);
        string Update(HoaDon obj);
        string Delete(int id);
    }
}
