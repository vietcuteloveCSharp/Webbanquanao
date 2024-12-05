using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IKhuyenMaiRepository
    {
        List<KhuyenMai> GetAll();
        KhuyenMai GetById(int id);
        bool Add(KhuyenMai obj);
        bool Update(KhuyenMai obj);
        bool Delete(int id);
    }
}
