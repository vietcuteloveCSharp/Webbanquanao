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
        Task<List<PhuongThucThanhToan>> GetAll();
        Task<PhuongThucThanhToan>  GetById(int id);
        Task<bool> Add(PhuongThucThanhToan obj);
        Task<bool> Update(PhuongThucThanhToan obj);
        Task<bool>  Delete(int id);    
    }
}
