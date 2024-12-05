using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface INhanVienRepository
    {
        Task<List<NhanVien>> GetAll();
        Task<NhanVien> GetById(int id);   
        Task<bool> Add(NhanVien obj);    
        Task<bool> Update(int id, NhanVien obj);
        Task<bool> Delete(int id);
    }
}
