using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IChiTietKhuyenMaiRepository
    {
        Task<List<ChiTietKhuyenMai>>GetAll();
        ChiTietKhuyenMai GetById(int id);
        bool Add(ChiTietKhuyenMai obj);
        bool Update(ChiTietKhuyenMai obj);
     

    }
}
