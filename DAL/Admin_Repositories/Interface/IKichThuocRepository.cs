using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IKichThuocRepository
    {
        List<KichThuoc> GetAll();
        KichThuoc GetById(int id);
        bool Add(KichThuoc obj);
        bool Update(KichThuoc obj);
        bool Delete(int id);
    }
}
