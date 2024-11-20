using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IPhuongThucThanhToanService
    {
        List<PhuongThucThanhToan> GetAll();
        PhuongThucThanhToan GetById(int id);
        string Add(PhuongThucThanhToan obj);
        string Update(PhuongThucThanhToan obj);
        string Delete(int id);
    }
}
