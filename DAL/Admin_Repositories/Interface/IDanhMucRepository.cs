using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IDanhMucRepository
    {
        Task<List<DanhMuc>> GetAll();
        Task<DanhMuc> GetById(int id);
        Task<bool> Add(DanhMuc obj);
        Task<bool> Update(int id,DanhMuc obj);
        Task<bool> Delete(int id);
    }
}
