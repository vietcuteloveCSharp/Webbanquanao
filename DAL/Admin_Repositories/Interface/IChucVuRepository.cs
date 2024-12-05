using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IChucVuRepository
    {
        Task<List<ChucVu>> GetAll();
        Task<ChucVu> GetById(int id);
        Task<bool> Add(ChucVu obj);
        Task<bool> Update(ChucVu obj);
        Task<bool> Delete(int id);
    }
}
