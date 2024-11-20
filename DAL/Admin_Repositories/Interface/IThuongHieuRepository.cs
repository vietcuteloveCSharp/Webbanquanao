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
        List<ThuongHieu> GetAll();
        ThuongHieu GetById(int id);
        bool Add(ThuongHieu obj);
        bool Update(ThuongHieu obj);
        bool Delete(int id);
    }
}
