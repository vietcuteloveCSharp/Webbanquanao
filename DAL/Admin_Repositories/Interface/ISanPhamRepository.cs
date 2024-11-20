using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface ISanPhamRepository
    {
        Task<List<SanPham>> GetAll();
        Task<SanPham> GetById(int id);
        Task<bool> Add(SanPham obj);
        Task<bool> Update(int id,SanPham obj);
        Task<bool> Delete(int id);

    }
}
