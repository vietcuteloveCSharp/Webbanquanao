using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IKhachHangRepository
    {
        List<KhachHang> GetAll();
        KhachHang GetById(int id);
        bool Add(KhachHang obj);
        bool Update(KhachHang obj);
        bool Delete(int id);
    }
}
