using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IPhuongThucThanhToanRepository
    {
        List<PhuongThucThanhToan> GetAll();
        PhuongThucThanhToan  GetById(int id);
        bool Add(PhuongThucThanhToan obj);
        bool Update(PhuongThucThanhToan obj);
        bool Delete(int id);    
    }
}
