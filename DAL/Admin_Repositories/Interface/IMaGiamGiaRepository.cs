using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IMaGiamGiaRepository
    {
        List<MaGiamGia> GetAll();
        MaGiamGia GetById(int id);
        bool Add(MaGiamGia obj);
        bool Update(MaGiamGia obj);
        bool Delete(int id);
    }
}
