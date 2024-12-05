using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IChiTietMaGiamRepository
    {
        List<ChiTietMaGiamGia> GetAll();
        ChiTietMaGiamGia GetById(int id);
        bool Add(ChiTietMaGiamGia obj);
        bool Update(ChiTietMaGiamGia obj);
      
    }
}
