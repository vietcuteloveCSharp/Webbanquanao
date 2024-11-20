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
        List<ChucVu> GetAll();
        ChucVu GetById(int id);
        bool Add(ChucVu obj);
        bool Update(ChucVu obj);
        bool Delete(int id);
    }
}
