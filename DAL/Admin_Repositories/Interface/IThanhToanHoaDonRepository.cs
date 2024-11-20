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
        Task<List<ThanhToanHoaDon>> GetAll();
        Task<ThanhToanHoaDon> GetById(int id);
        Task<bool> Add(ThanhToanHoaDon obj);
        Task<bool> Update(int id,ThanhToanHoaDon obj);
      
    }
}
