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
        List<SanPham> GetAll();
        SanPham GetById(int id);
        bool Add(SanPham obj);
        bool Update(SanPham obj);
        bool Delete(int id);

    }
}
