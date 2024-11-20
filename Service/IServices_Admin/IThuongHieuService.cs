using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IThuongHieuService
    {
        List<ThuongHieu> GetAll(ThuongHieu obj);
        ThuongHieu GetById(int id);
        string Add (ThuongHieu obj);    
        string Update (ThuongHieu obj);
        string Delete (int id);
    }
}
