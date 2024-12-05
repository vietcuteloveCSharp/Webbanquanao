using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IChiTietHoaDonRepository
    {
        List<ChiTietHoaDon> GetAll();
        ChiTietHoaDon GetById(int id);
        bool Add(ChiTietHoaDon obj);
        bool Update(ChiTietHoaDon obj);

    }
}
