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
        Task<List<HoaDon>> GetAll();
        Task<HoaDon> GetById(int id);
        Task<bool> Add(HoaDon obj);    
        Task<bool> Update(int id,HoaDon obj);
        Task<bool> Delete(int id);
    }
}
