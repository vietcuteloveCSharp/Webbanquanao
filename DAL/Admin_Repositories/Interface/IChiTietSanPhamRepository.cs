using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IChiTietSanPhamRepository
    {
        List<ChiTietSanPham> GetAll();
        ChiTietSanPham Get(int id);
        bool Add(ChiTietSanPham obj);
        bool Update(ChiTietSanPham obj); 
        
    }
}
