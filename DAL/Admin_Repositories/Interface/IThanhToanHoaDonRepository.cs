using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IThanhToanHoaDonRepository
    {
        List<ThanhToanHoaDon> GetAll();
        ThanhToanHoaDon GetById(int id);
        bool Add(ThanhToanHoaDon obj);
        bool Update(ThanhToanHoaDon obj);
      
    }
}
