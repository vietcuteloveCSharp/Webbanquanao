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
        List<DanhMuc> GetAll();
        DanhMuc GetById(int id);
        bool Add(DanhMuc obj);
        bool Update(DanhMuc obj);
        bool Delete(int id);
    }
}
