using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IThuongHieuRepository
    {
        Task<List<ThuongHieu>> GetAll();
        Task<ThuongHieu> GetById(int id);
        Task<bool> Add(ThuongHieu obj);
        Task<bool> Update(int id,ThuongHieu obj);
        Task<bool> Delete(int id);
    }
}
