using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IThanhToanHoaDonService
    {
        List<ThanhToanHoaDon> GetAll();
        ThanhToanHoaDon GetById(int id);
        string Add(ThanhToanHoaDon obj);
        string Update(ThanhToanHoaDon obj);

    }
}
